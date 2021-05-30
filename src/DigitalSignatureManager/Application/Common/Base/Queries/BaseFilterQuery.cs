using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Application.Common.Models;
using Domain.Common;
using Domain.Common.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Common.Base.Queries
{
    public abstract class BaseFilterQuery<TEntity, TModel, TFilter> where TEntity : class, IEntity where TFilter : FilterPagingQueryModel
    {
        private readonly ILogger<BaseFilterQuery<TEntity, TModel, TFilter>> _logger;
        private readonly IDigitalSignatureManagerDbContext _context;

        protected BaseFilterQuery(ILogger<BaseFilterQuery<TEntity, TModel, TFilter>> logger, IDigitalSignatureManagerDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<PagingResultModel<TModel>> Query(TFilter model)
        {
            _logger.LogInformation($"Querying {typeof(TEntity).Name}(s); page {model.Page}; size {model.Size}");

            var totalCount = await _context.EntitySet<TEntity>().Where(GetFilterExpression(model)).CountAsync();

            var entities = await _context.EntitySet<TEntity>()
                .AsNoTracking()
                .Where(GetFilterExpression(model))
                .Skip((model.Page - 1) * model.Size)
                .Take(model.Size)
                .OrderBy(x => x.Id)
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

        protected abstract Expression<Func<TEntity, bool>> GetFilterExpression(TFilter filterModel);
        protected abstract Expression<Func<TEntity, TModel>> GetMappingExpression();
    }
}