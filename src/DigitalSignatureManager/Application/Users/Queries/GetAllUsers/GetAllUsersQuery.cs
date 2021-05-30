using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common.Base.Queries;
using Application.Common.Contracts;
using Application.Common.Models;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetAllUsers
{
    public interface IGetAllUsersQuery
    {
        Task<PagingResultModel<IdNameModel>> Query(PagingQueryModel model);
    }

    public class GetAllUsersQuery : BasePagingQuery<ApplicationUser, IdNameModel>, IGetAllUsersQuery
    {
        public GetAllUsersQuery(ILogger<BasePagingQuery<ApplicationUser, IdNameModel>> logger, IDigitalSignatureManagerDbContext context) : base(logger, context)
        {
        }

        protected override Expression<Func<ApplicationUser, IdNameModel>> GetMappingExpression() =>
            u => new IdNameModel
            {
                Id = u.Id,
                Name = u.UserName
            };
    }
}