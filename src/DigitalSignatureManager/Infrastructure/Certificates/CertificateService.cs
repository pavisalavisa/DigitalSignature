using System.Threading.Tasks;
using Application.Common.Contracts;
using Application.Common.ErrorManagement.ErrorCodes;
using Application.Common.ErrorManagement.Exceptions;
using Application.Common.Models.DigitalSignatureModels;
using Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Certificates
{
    public class CertificateService : ICertificateService
    {
        private readonly ILogger<CertificateService> _logger;
        private readonly IDigitalSignatureManagerDbContext _context;
        private readonly string _encryptionKey;

        public CertificateService(ILogger<CertificateService> logger, IDigitalSignatureManagerDbContext context, IConfiguration configuration)
        {
            _logger = logger;
            _context = context;
            _encryptionKey = configuration.GetValue<string>("EncryptionKey");
            if (_encryptionKey is null)
            {
                throw new ConfigurationException("EncryptionKey");
            }
        }

        public async Task<InternalCertificateSignatureModel> GetUserCertificate(int userId)
        {
            _logger.LogInformation($"Fetching certificate for user with id {userId}");

            var certificate = await _context.Certificates.FirstOrDefaultAsync(c => c.OwnerId == userId);
            if (certificate is null)
            {
                _logger.LogError($"User with id {userId} does not have a certificate");
                throw new BusinessException(CertificateErrorCodes.UserCertificateDoesntExist);
            }

            return new InternalCertificateSignatureModel
            {
                B64Certificate = CryptographyHelper.Decrypt(certificate.B64Certificate, _encryptionKey),
                CertificatePassword = CryptographyHelper.Decrypt(certificate.B64Password, _encryptionKey)
            };
        }
    }
}