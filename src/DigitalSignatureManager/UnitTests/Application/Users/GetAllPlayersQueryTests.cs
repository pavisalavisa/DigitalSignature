using System.Collections.Generic;
using Application.Common.Models;
using Application.Users.Queries.GetAllUsers;
using Domain.Common;
using NUnit.Framework;
using UnitTests.Application.Base;

namespace UnitTests.Application.Users
{
    [TestFixture]
    public class GetAllUsersQueryTests : BaseGetAllEntitiesQueryTests<ApplicationUser, IdNameModel, GetAllUsersQuery>
    {
        protected override IEnumerable<ApplicationUser> GetSeedEntities()
        {
            yield return new ApplicationUser{Email ="joe1@gmail.com"};
            yield return new ApplicationUser{Email ="joe2@gmail.com"};
            yield return new ApplicationUser{Email ="joe3@gmail.com"};
            yield return new ApplicationUser{Email ="joe4@gmail.com"};
            yield return new ApplicationUser{Email ="joe5@gmail.com"};
            yield return new ApplicationUser{Email ="joe6@gmail.com"};
            yield return new ApplicationUser{Email ="joe7@gmail.com"};
            yield return new ApplicationUser{Email ="joe8@gmail.com"};
            yield return new ApplicationUser{Email ="joe9@gmail.com"};
            yield return new ApplicationUser{Email ="joe10@gmail.com"};
            yield return new ApplicationUser{Email ="joe11@gmail.com"};
        }

        protected override GetAllUsersQuery ConstructQuery() => new(_logger.Object, _context);
    }
}