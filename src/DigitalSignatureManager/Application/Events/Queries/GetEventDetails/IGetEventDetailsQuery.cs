using System.Threading.Tasks;

namespace Application.Events.Queries.GetEventDetails
{
    public interface IGetEventDetailsQuery
    {
        Task<EventDetailsModel> Query(int id);
    }
}