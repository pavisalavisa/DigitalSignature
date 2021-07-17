using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Contracts;
using Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetPersonalInformation
{
    public class GetPersonalInformationQuery : IGetPersonalInformationQuery
    {
        private readonly ILogger<GetPersonalInformationQuery> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDigitalSignatureManagerDbContext _context;

        public GetPersonalInformationQuery(ILogger<GetPersonalInformationQuery> logger,
            IHttpContextAccessor httpContextAccessor, IDigitalSignatureManagerDbContext context)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<PersonalInformationModel> Query()
        {
            var userId = _httpContextAccessor.GetUserIdFromClaims();
            _logger.LogDebug("Getting personal information for user with id {UserId}", userId);

            var entity = await _context.EntitySet<ApplicationUser>()
                .AsNoTracking()
                .Where(x => x.Id == userId)
                .Select(GetMappingExpression())
                .FirstOrDefaultAsync();

            if (entity is null)
                _logger.LogInformation("User with with id {Id} does not exist", userId);

            return entity;
        }

        private Expression<Func<ApplicationUser, PersonalInformationModel>> GetMappingExpression() =>
            x => new PersonalInformationModel
            {
                Email = x.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                OrganizationName = x.OrganizationName
            };
    }
}