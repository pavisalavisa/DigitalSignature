using System.Threading.Tasks;
using Application.Common.Contracts;
using Domain.Entities;

namespace Infrastructure.Events
{
    public class EventService : IEventService
    {
        private readonly IDigitalSignatureManagerDbContext _context;

        public EventService(IDigitalSignatureManagerDbContext context)
        {
            _context = context;
        }

        public async Task RecordEvent(Event @event)
        {
            await _context.Events.AddAsync(@event);

            await _context.SaveChangesAsync();
        }
    }
}