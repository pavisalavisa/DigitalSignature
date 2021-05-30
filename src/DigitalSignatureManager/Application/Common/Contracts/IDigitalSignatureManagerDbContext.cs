using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Contracts
{
    public interface IDigitalSignatureManagerDbContext
    {
        DbSet<T> EntitySet<T>() where T : class, IEntity;
        IQueryable<T> EagerlyLoadedEntities<T>() where T : class, IEntity;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}