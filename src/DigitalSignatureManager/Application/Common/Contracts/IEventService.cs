using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Common.Contracts
{
    public interface IEventService
    {
        Task RecordEvent(Event @event);
    }
}