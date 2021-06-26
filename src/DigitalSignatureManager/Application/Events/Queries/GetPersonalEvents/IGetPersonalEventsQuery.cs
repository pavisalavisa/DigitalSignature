using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Events.Queries.GetPersonalEvents
{
    public interface IGetPersonalEventsQuery
    {
        Task<PagingResultModel<PersonalEventModel>> Query(FilterPagingQueryModel model);
    }
}