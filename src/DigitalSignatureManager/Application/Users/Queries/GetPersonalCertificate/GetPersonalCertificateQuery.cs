using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Contracts;
using Application.Common.ErrorManagement.Exceptions;
using Application.Users.Queries.GetPersonalInformation;
using Domain.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Users.Queries.GetPersonalCertificate
{
    public class GetPersonalCertificateQuery : IGetPersonalCertificateQuery
    {
        private readonly ILogger<GetPersonalInformationQuery> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDigitalSignatureManagerDbContext _context;

        public GetPersonalCertificateQuery(ILogger<GetPersonalInformationQuery> logger,
            IHttpContextAccessor httpContextAccessor, IDigitalSignatureManagerDbContext context)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public async Task<Stream> Query()
        {
            var userId = _httpContextAccessor.GetUserIdFromClaims();
            _logger.LogDebug("Getting personal certificate for user with id {UserId}", userId);

            var cert = await _context.Certificates
                .AsNoTracking()
                .Where(x => x.OwnerId == userId)
                .Select(x=>x.B64Certificate)
                .FirstOrDefaultAsync();

            if (cert is null)
            {
                _logger.LogInformation("Certificate with with id {Id} does not exist", userId);
                throw new NotFoundException("Personal certificate does not exist.");
            }
                
            return new MemoryStream(Convert.FromBase64String(cert));
        }
    }
}