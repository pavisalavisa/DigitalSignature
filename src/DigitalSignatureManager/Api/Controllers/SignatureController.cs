using System.Threading.Tasks;
using Application.Signature.Queries.GetSignatureServiceHealth;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class SignatureController : BaseController
    {
        /// <summary>
        /// Gets the health of the underlying signature service
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Health")]
        public async Task<IActionResult> GetHealth([FromServices] IGetSignatureServiceHealth query)
        {
            var isHealthy = await query.IsHealthy();

            return new OkObjectResult(new {isHealthy});
        }
    }
}