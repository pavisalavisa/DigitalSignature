using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Domain.Common;
using Domain.Common.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Common.Base.Queries
{
    public abstract class BaseIdQuery<TEntity, TModel> where TEntity : class, IEntity
    {
        private readonly ILogger<BaseIdQuery<TEntity, TModel>> _logger;
        private readonly IDigitalSignatureManagerDbContext _context;

        protected BaseIdQuery(ILogger<BaseIdQuery<TEntity, TModel>> logger, IDigitalSignatureManagerDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<TModel> Query(int id)
        {
            _logger.LogInformation($"Querying {typeof(TEntity).Name}(s) with id {id}");

            var entity = await _context.EagerlyLoadedEntities<TEntity>()
                .AsNoTracking()
                .Where(x=>x.Id == id)
                .Select(GetMappingExpression())
                .FirstOrDefaultAsync();

            if (entity is null)
                _logger.LogInformation($"Entity {typeof(TEntity).Name} with id {id} does not exist.");

            return entity;
        }

        protected abstract Expression<Func<TEntity,TModel>> GetMappingExpression();
    }
}