using System.Threading.Tasks;
using Application.Common.Contracts;
using Domain.Common;
using Domain.Common.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Common.Base.Commands
{
    public abstract class BaseDeleteCommand<TEntity> where TEntity : class, IEntity, new()
    {
        protected readonly ILogger<BaseDeleteCommand<TEntity>> _logger;
        protected readonly IDigitalSignatureManagerDbContext _context;

        protected BaseDeleteCommand(IDigitalSignatureManagerDbContext context, ILogger<BaseDeleteCommand<TEntity>> logger)
        {
            _context = context;
            _logger = logger;
        }

        public virtual async Task Execute(int id)
        {
            _logger.LogInformation($"Deleting entity {typeof(TEntity).Name} with id {id}");

            try
            {
                _context.EntitySet<TEntity>().Remove(new TEntity {Id = id});

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Successfully deleted entity {typeof(TEntity).Name} with id {id}");
            }
            catch (DbUpdateConcurrencyException)
            {
                _logger.LogWarning($"Entity with id {id} does not exist and cannot be deleted.");
            }
        }
    }
}