using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Domain.Common.Base;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Contracts
{
    public interface IDigitalSignatureManagerDbContext
    {
        DbSet<Event> Events { get; }
        DbSet<Certificate> Certificates { get; }

        DbSet<T> EntitySet<T>() where T : class, IEntity;
        IQueryable<T> EagerlyLoadedEntities<T>() where T : class, IEntity;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}