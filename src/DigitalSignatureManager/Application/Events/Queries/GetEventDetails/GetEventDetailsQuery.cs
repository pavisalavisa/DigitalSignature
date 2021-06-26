using System.Linq;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.Events.Queries.GetEventDetails
{
    public class GetEventDetailsQuery : IGetEventDetailsQuery
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetEventDetailsQuery> _logger;
        private readonly IDigitalSignatureManagerDbContext _context;

        public GetEventDetailsQuery(IHttpContextAccessor httpContextAccessor, ILogger<GetEventDetailsQuery> logger,
            IDigitalSignatureManagerDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _context = context;
        }

        public async Task<EventDetailsModel> Query(int id)
        {
            _logger.LogInformation($"Querying event with id {id}");

            var userId = _httpContextAccessor.GetUserIdFromClaims();

            var entity = await _context.Events
                .AsNoTracking()
                .Where(x => x.Id == id && x.TriggeredById == userId)
                .Select(x => new EventDetailsModel
                {
                    Id = x.Id,
                    Created = x.Created,
                    Error = x.Error,
                    Type = x.Type,
                    IsSuccessful = x.IsSuccessful,
                    InputDocumentName = x.InputDocumentName,
                    InputDocumentB64 = x.InputDocumentB64,
                    OutpuDocumentB64 = x.OutputDocumentB64
                })
                .FirstOrDefaultAsync();

            if (entity is null)
                _logger.LogInformation($"Event with id {id} does not exist for user with id {userId}.");

            return entity;
        }
    }
}