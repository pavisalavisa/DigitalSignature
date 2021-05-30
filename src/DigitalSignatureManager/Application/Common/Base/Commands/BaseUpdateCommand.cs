using System;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Common.Base.Commands
{
    public abstract class BaseUpdateCommand<TEntity, TModel> where TEntity : class, IEntity
    {
        protected readonly ILogger<BaseUpdateCommand<TEntity, TModel>> _logger;
        protected readonly IDigitalSignatureManagerDbContext _context;

        protected BaseUpdateCommand(IDigitalSignatureManagerDbContext context, ILogger<BaseUpdateCommand<TEntity, TModel>> logger)
        {
            _context = context;
            _logger = logger;
        }

        protected virtual Task Validate(TModel model, int id)
        {
            if (model is null)
                throw new ArgumentNullException(nameof(model));

            return Task.CompletedTask;
        }

        protected abstract Task ApplyChanges(TEntity entity, TModel model);

        public virtual async Task Execute(TModel model, int id)
        {
            _logger.LogInformation($"Updating entity {typeof(TEntity).Name} with id {id}");

            await Validate(model, id);

            var entity = await _context.EntitySet<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
            if (entity is null)
                throw new ApplicationException($"Entity {typeof(TEntity).Name} does not exist.");

            await ApplyChanges(entity,model);

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Successfully updated entity {typeof(TEntity).Name} with id {entity.Id}");
        }
    }
}