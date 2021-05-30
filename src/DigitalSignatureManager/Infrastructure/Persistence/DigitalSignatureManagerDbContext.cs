using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Contracts;
using Domain.Common;
using Domain.Common.Base;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class DigitalSignatureManagerDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>, IDigitalSignatureManagerDbContext
    {
        public DigitalSignatureManagerDbContext(DbContextOptions<DigitalSignatureManagerDbContext> options) : base(options)
        {
        }

        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<T> EntitySet<T>() where T : class, IEntity => Set<T>();

        public IQueryable<T> EagerlyLoadedEntities<T>() where T : class, IEntity
        {
            var query = Set<T>().AsQueryable();

            var navigations = Model.FindEntityType(typeof(T))
                .GetDerivedTypesInclusive()
                .SelectMany(type => type.GetNavigations())
                .Distinct();

            foreach (var property in navigations)
                query = query.Include(property.Name);

            return query;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            // Run interceptors, dispatch events, ...
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }
    }
}