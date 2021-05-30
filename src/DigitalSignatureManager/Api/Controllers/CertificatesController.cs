using System.Threading.Tasks;
using Application.Certificates.Queries;
using Application.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class CertificatesController : BaseController
    {
        /// <summary>
        /// Gets the list of all certificates
        /// </summary>
        /// <remarks>
        /// Gets the list of all certificates with filtering and pagination.
        ///
        /// Sample request:
        ///
        ///     GET /api/Certificates?IsRevoked=true&amp;UpdatedFrom=2021-04-11T09:02:05Z
        ///
        /// </remarks>
        /// <param name="filterModel">Filter and pagination options</param>
        /// <returns>Returns paginated list of certificates</returns>
        /// <response code ="200">Certificates were successfully fetched.</response>
        /// <response code ="400">Validation error happened</response>
        [HttpGet]
        [Produces("application/json")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagingResultModel<CertificateModel>>> Get(
            [FromQuery] CertificateFilterModel filterModel,
            [FromServices] IGetAllCertificatesQuery query)
        {
            return Ok(await query.Query(filterModel));
        }
    }
}