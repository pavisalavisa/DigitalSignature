using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Base.Queries;
using Application.Common.Models;
using Domain.Common;
using Domain.Common.Base;
using Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace UnitTests.Application.Base
{
    public abstract class BaseGetAllEntitiesQueryTests<TEntity, TModel, TQuery> : BaseTest where TEntity : class, IEntity
        where TQuery : BasePagingQuery<TEntity, TModel>
        where TModel : class
    {
        protected Mock<ILogger<BasePagingQuery<TEntity, TModel>>> _logger;
        protected DigitalSignatureManagerDbContext _context;

        private IList<TEntity> _seededPlayers;

        private TQuery _query;

        [SetUp]
        public new void SetUp()
        {
            _logger = new Mock<ILogger<BasePagingQuery<TEntity, TModel>>>();
            _context = new DigitalSignatureManagerDbContext(ContextOptions);

            _seededPlayers = GetSeedEntities().ToList();
            Seed(_seededPlayers.ToArray());

            _query = ConstructQuery();
        }

        [Test]
        public async Task Query_ShouldReturnPaginationModel()
        {
            var model = new PagingQueryModel();
            var result = await _query.Query(model);

            Assert.AreEqual(20, result.PageSize);
            Assert.AreEqual(1, result.CurrentPage);
            Assert.AreEqual(_seededPlayers.Count, result.Count);
            Assert.LessOrEqual(result.Items.Count(),20);
        }

        protected abstract IEnumerable<TEntity> GetSeedEntities();
        protected abstract TQuery ConstructQuery();
    }
}