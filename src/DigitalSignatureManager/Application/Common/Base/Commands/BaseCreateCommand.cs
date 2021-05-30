using System;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Application.Common.Models;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Application.Common.Base.Commands
{
    public abstract class BaseCreateCommand<TEntity, TModel> where TEntity : class, IEntity
    {
        protected readonly ILogger<BaseCreateCommand<TEntity, TModel>> _logger;
        protected readonly IDigitalSignatureManagerDbContext _context;

        protected BaseCreateCommand(IDigitalSignatureManagerDbContext context, ILogger<BaseCreateCommand<TEntity, TModel>> logger)
        {
            _context = context;
            _logger = logger;
        }

        protected virtual Task Validate(TModel model)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));
            
            return Task.CompletedTask;
        }

        protected abstract TEntity MapToEntity(TModel model);

        public virtual async Task<EntityCreatedModel> Execute(TModel model)
        {
            _logger.LogInformation($"Creating new entity {typeof(TModel).Name}");

            await Validate(model);
            var entity = MapToEntity(model);

            await _context.EntitySet<TEntity>().AddAsync(entity);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Successfully created entity {typeof(TEntity).Name} with id {entity.Id}");

            return new EntityCreatedModel(entity.Id);
        }
    }
}