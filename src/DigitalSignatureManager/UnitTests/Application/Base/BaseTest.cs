using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace UnitTests.Application.Base
{
    public abstract class BaseTest
    {
        protected DbContextOptions<DigitalSignatureManagerDbContext> ContextOptions;
        
        private const string TestDbName = "TestDigitalSignatureManagerDatabase";

        protected BaseTest()
        {
            ContextOptions = new DbContextOptionsBuilder<DigitalSignatureManagerDbContext>()
                .UseInMemoryDatabase(TestDbName).Options;
        }

        [SetUp]
        public void SetUp()
        {
            using var context = new DigitalSignatureManagerDbContext(ContextOptions);

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        
        protected void Seed<T>(params T[] entities)
        {
            using var context = new DigitalSignatureManagerDbContext(ContextOptions);
            {
                foreach (var entity in entities)
                {
                    context.Add(entity);
                }
                
                context.SaveChanges();
            }
        }
    }
}