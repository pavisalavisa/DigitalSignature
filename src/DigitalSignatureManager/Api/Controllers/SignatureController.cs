using System.Threading.Tasks;
using Application.Signature.Commands.SignBinary;
using Application.Signature.Commands.SignPdf;
using Application.Signature.Queries.GetSignatureServiceHealth;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Signs the provided b64 encoded pdf file with certificate attached to current user
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "RegularUser")]
        [Route("Pdf")]
        public async Task<ActionResult<SignedPdfResponseModel>> SignPdf(SignPdfModel requestModel, [FromServices] ISignPdfCommand command)
        {
            var response = await command.Execute(requestModel);

            return new OkObjectResult(response);
        }

        /// <summary>
        /// Signs the provided b64 encoded binary file with certificate attached to current user
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "RegularUser")]
        [Route("Binary")]
        public async Task<ActionResult<SignedBinaryResponseModel>> SignBinary(SignBinaryModel requestModel, [FromServices] ISignBinaryCommand command)
        {
            var response = await command.Execute(requestModel);

            return new OkObjectResult(response);
        }
    }
}