using System;
using System.Linq.Expressions;
using Application.Common.Base.Queries;
using Application.Common.Contracts;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : BaseIdQuery<ApplicationUser, UserModel>, IGetUserByIdQuery
    {
        public GetUserByIdQuery(ILogger<BaseIdQuery<ApplicationUser, UserModel>> logger, IDigitalSignatureManagerDbContext context) : base(logger, context)
        {
        }

        protected override Expression<Func<ApplicationUser, UserModel>> GetMappingExpression() =>
            u => new UserModel
            {
                Email = u.Email,
                Id = u.Id,
                Name = u.UserName
            };
    }
}