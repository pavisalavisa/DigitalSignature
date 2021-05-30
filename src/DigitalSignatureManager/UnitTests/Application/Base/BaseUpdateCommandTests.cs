using System;
using System.Collections.Generic;
using System.Linq;
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
    public abstract class BaseUpdateCommandTests<TEntity, TModel, TCommand> : BaseTest where TEntity : class, IEntity
        where TCommand : BaseUpdateCommand<TEntity, TModel>
        where TModel : class
    {
        protected Mock<ILogger<BaseUpdateCommand<TEntity, TModel>>> _logger;
        protected DigitalSignatureManagerDbContext _context;

        private TCommand _command;

        [SetUp]
        public new void SetUp()
        {
            _logger = new Mock<ILogger<BaseUpdateCommand<TEntity, TModel>>>();
            _context = new DigitalSignatureManagerDbContext(ContextOptions);

            Seed(GetSeedEntities().ToArray());
            
            _command = ConstructCommand();
        }

        [Test]
        public void Execute_WithNullModel_ShouldThrowAnException()
        {
            AsyncTestDelegate a = async () => await _command.Execute(null, 1);

            Assert.ThrowsAsync<ArgumentNullException>(a);
        }
        
        [Test]
        public void Execute_WithNonexistentEntity_ShouldThrowAnException()
        {
            AsyncTestDelegate a = async () => await _command.Execute(GetValidModel(), 144);

            Assert.ThrowsAsync<ApplicationException>(a);
        }

        [Test]
        public async Task Execute_ShouldSaveUpdateChangesToDatabase()
        {
            await _command.Execute(GetValidModel(), 1);

            var updatedEntity = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == 1);

            UpdateAssertions(updatedEntity);
        }

        protected abstract IEnumerable<TEntity> GetSeedEntities();
        protected abstract TCommand ConstructCommand();
        protected abstract TModel GetValidModel();
        protected abstract void UpdateAssertions(TEntity updatedEntity);
    }
}