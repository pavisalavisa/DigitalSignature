using System.Threading.Tasks;
using Application.Common.Base.Commands;
using Domain.Common;
using Domain.Common.Base;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests.Application.Base
{
    public abstract class BaseDeleteCommandTests<TEntity, TCommand> : BaseTest where TEntity : class, IEntity, new()
        where TCommand : BaseDeleteCommand<TEntity>
    {
        protected Mock<ILogger<BaseDeleteCommand<TEntity>>> _logger;
        protected DigitalSignatureManagerDbContext _context;

        private TCommand _command;

        [SetUp]
        public new void SetUp()
        {
            Seed(GetValidEntity());
            
            _logger = new Mock<ILogger<BaseDeleteCommand<TEntity>>>();
            _context = new DigitalSignatureManagerDbContext(ContextOptions);

            _command = ConstructCommand();

            base.SetUp();
        }

        [Test]
        public void Execute_WithNonExistentEntity_ShouldNotThrowAnException()
        {
            AsyncTestDelegate a = async () => await _command.Execute(1289);

            Assert.DoesNotThrowAsync(a);
        }

        [Test]
        public async Task Execute_WithExistingEntity_ShouldDeleteIt()
        {
            await _command.Execute(1);

            Assert.False(await _context.Set<TEntity>().AnyAsync(x=>x.Id == 1));
        }

        protected abstract TCommand ConstructCommand();
        protected abstract TEntity GetValidEntity();
    }
}