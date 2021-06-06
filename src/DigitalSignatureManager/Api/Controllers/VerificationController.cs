using System.Threading.Tasks;
using Application.Verification.Commands.VerifyBinary;
using Application.Verification.Commands.VerifyPdf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class VerificationController : BaseController
    {
        /// <summary>
        /// Verifies the provided b64 encoded pdf which may have 0 or more PAdES signatures
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("RegularUser")]
        [Route("Pdf")]
        public async Task<IActionResult> VerifyPdf(VerifyPdfModel requestModel, [FromServices] IVerifyPdfCommand command)
        {
            var response = await command.Execute(requestModel);

            return new OkObjectResult(response);
        }

        /// <summary>
        /// Verifies the detached signature for the provided file
        /// </summary>
        /// <param name="requestModel"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize("RegularUser")]
        [Route("Binary")]
        public async Task<IActionResult> VerifyBinary(VerifyBinaryModel requestModel, [FromServices] IVerifyBinaryCommand command)
        {
            var response = await command.Execute(requestModel);

            return new OkObjectResult(response);
        }
    }
}