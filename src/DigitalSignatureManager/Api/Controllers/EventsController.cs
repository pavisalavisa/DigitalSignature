using System.Threading.Tasks;
using Application.Common.Models;
using Application.Events.Queries.GetEventDetails;
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

        /// <summary>
        /// Gets the details of a particular event
        /// </summary>
        /// <remarks>
        /// Gets the event with provided id for an authenticated user.
        /// 
        /// Sample request:
        /// 
        ///     GET /api/Events/3
        /// 
        /// </remarks>
        /// <param name="id">Id of the event</param>
        /// <param name="query"></param>
        /// <returns>Returns paginated list of personal events</returns>
        /// <response code ="200">Event details were successfully fetched.</response>
        /// <response code ="400">Validation error happened</response>
        /// <response code ="404">Event doesn't exist</response>
        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = "RegularUser")]
        [Route("{id}")]
        public async Task<ActionResult<PagingResultModel<PersonalEventModel>>> Get(
            [FromRoute] int id,
            [FromServices] IGetEventDetailsQuery query)
        {
            var @event = await query.Query(id);

            if (@event is null)
                return NotFound();

            return Ok(@event);
        }
    }
}