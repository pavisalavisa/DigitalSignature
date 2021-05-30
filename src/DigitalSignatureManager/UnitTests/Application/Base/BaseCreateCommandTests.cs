using System;
using System.Threading.Tasks;
using Application.Common.Base.Commands;
using Domain.Common;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests.Application.Base
{
    public abstract class BaseCreateCommandTests<TEntity, TModel, TCommand> : BaseTest where TEntity : class, IEntity
        where TCommand : BaseCreateCommand<TEntity, TModel>
        where TModel : class
    {
        protected Mock<ILogger<BaseCreateCommand<TEntity, TModel>>> _logger;
        protected DigitalSignatureManagerDbContext _context;

        private TCommand _command;

        [SetUp]
        public new void SetUp()
        {
            _logger = new Mock<ILogger<BaseCreateCommand<TEntity, TModel>>>();
            _context = new DigitalSignatureManagerDbContext(ContextOptions);

            _command = ConstructCommand();

            base.SetUp();
        }

        [Test]
        public void Execute_WithNullModel_ShouldThrowAnException()
        {
            AsyncTestDelegate a = async () => await _command.Execute(null);

            Assert.ThrowsAsync<ArgumentNullException>(a);
        }


        [Test]
        public async Task Execute_ShouldReturnEntityCreatedModelId()
        {
            var result = await _command.Execute(GetValidModel());

            Assert.AreEqual(1, result.Id);
        }

        [Test]
        public async Task Execute_ShouldSaveModelToDatabase()
        {
            var result = await _command.Execute(GetValidModel());

            Assert.True(await _context.Set<TEntity>().AnyAsync());
        }

        protected abstract TCommand ConstructCommand();
        protected abstract TModel GetValidModel();
    }
}