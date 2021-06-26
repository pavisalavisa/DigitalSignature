using System.Threading.Tasks;
using Application.Common.Models;
using Application.Events.Queries.GetPersonalEvents;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class EventsController : BaseController
    {
        /// <summary>
        /// Gets the list of all events for authenticated user
        /// </summary>
        /// <remarks>
        /// Gets the list of all events with filtering and pagination.
        /// 
        /// Sample request:
        /// 
        ///     GET /api/Events?page=3
        /// 
        /// </remarks>
        /// <param name="filterModel">Filter and pagination options</param>
        /// <param name="query"></param>
        /// <returns>Returns paginated list of personal events</returns>
        /// <response code ="200">Events were successfully fetched.</response>
        /// <response code ="400">Validation error happened</response>
        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = "RegularUser")]
        public async Task<ActionResult<PagingResultModel<PersonalEventModel>>> Get(
            [FromQuery] FilterPagingQueryModel filterModel,
            [FromServices] IGetPersonalEventsQuery query)
        {
            return Ok(await query.Query(filterModel));
        }
    }
}