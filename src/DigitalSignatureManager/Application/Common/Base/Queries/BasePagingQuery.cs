using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Application.Common.Models;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Common.Base.Queries
{
    public abstract class BasePagingQuery<TEntity, TModel> where TEntity : class, IEntity
    {
        private readonly ILogger<BasePagingQuery<TEntity, TModel>> _logger;
        private readonly IDigitalSignatureManagerDbContext _context;

        protected BasePagingQuery(ILogger<BasePagingQuery<TEntity, TModel>> logger, IDigitalSignatureManagerDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<PagingResultModel<TModel>> Query(PagingQueryModel model)
        {
            _logger.LogInformation($"Querying {typeof(TEntity).Name}(s); page {model.Page}; size {model.Size}");

            var totalCount = await _context.EntitySet<TEntity>().CountAsync();

            var entities = await _context.EntitySet<TEntity>()
                .AsNoTracking()
                .Skip((model.Page - 1) * model.Size)
                .Take(model.Size)
                .Select(GetMappingExpression())
                .ToListAsync();

            return new PagingResultModel<TModel>
            {
                Count = totalCount,
                CurrentPage = model.Page,
                PageSize = model.Size,
                Items = entities
            };
        }

        protected abstract Expression<Func<TEntity, TModel>> GetMappingExpression();
    }
}