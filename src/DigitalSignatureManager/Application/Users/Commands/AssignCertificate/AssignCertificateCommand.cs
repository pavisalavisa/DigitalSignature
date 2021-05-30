using System.Threading.Tasks;
using Application.Common;
using Application.Common.Contracts;
using Application.Common.ErrorManagement.Exceptions;
using Common.Helpers;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Users.Commands.AssignCertificate
{
    public class AssignCertificateCommand : IAssignCertificateCommand
    {
        private readonly ILogger<AssignCertificateCommand> _logger;
        private readonly IDigitalSignatureManagerDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _encryptionKey;

        public AssignCertificateCommand(ILogger<AssignCertificateCommand> logger, IDigitalSignatureManagerDbContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _encryptionKey = configuration.GetSection("EncryptionKey").Value;
            if (_encryptionKey is null)
            {
                throw new ConfigurationException("EncryptionKey");
            }
        }

        public async Task Execute(CertificateAssignmentModel model)
        {
            var userId = _httpContextAccessor.GetUserIdFromClaims();

            _logger.LogInformation($"Assigning certificate for user with id {userId}");

            var previousCertificate = await _context.Certificates.FirstOrDefaultAsync(x => x.OwnerId == userId);
            if (previousCertificate is null)
            {
                await AddNewCertificate(model, userId);
            }
            else
            {
                UpdateExistingCertificate(previousCertificate, model);
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Certificate assigned  certificate for user with id {userId}");
        }

        private async Task AddNewCertificate(CertificateAssignmentModel model, int userId)
        {
            _logger.LogInformation($"Adding new certificate for user with id {userId}.");

            var newCertificate = CreateCertificate(model, userId);
            await _context.Certificates.AddAsync(newCertificate);
        }

        private void UpdateExistingCertificate(Certificate previousCertificate, CertificateAssignmentModel model)
        {
            _logger.LogInformation($"Updating existing certificate for user with id {previousCertificate.OwnerId}.");

            var newCertificate = CreateCertificate(model, previousCertificate.OwnerId);

            previousCertificate.B64Certificate = newCertificate.B64Certificate;
            previousCertificate.B64Password = newCertificate.B64Certificate;
        }

        private Certificate CreateCertificate(CertificateAssignmentModel model, int userId) =>
            new()
            {
                B64Certificate = CryptographyHelper.Encrypt(model.B64Certificate, _encryptionKey),
                B64Password = CryptographyHelper.Encrypt(model.CertificatePassword, _encryptionKey),
                IsRevoked = false,
                OwnerId = userId
            };
    }
}