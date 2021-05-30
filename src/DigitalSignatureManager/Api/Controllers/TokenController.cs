using System.Threading.Tasks;
using Api.Authentication;
using Api.Authentication.Contracts;
using Application.Common.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// Token related actions. 
    /// </summary>
    public class TokenController : BaseController
    {
        private readonly IJwtTokenProvider _tokenProvider;
        private readonly IApplicationUserManager _userManager;

#pragma warning disable 1591
        public TokenController(IJwtTokenProvider tokenProvider, IApplicationUserManager userManager)
#pragma warning restore 1591
        {
            _tokenProvider = tokenProvider;
            _userManager = userManager;
        }

        /// <summary>
        /// Authenticates the user and returns the JWT.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Token
        ///     {
        ///        "email": "jon.doe@gmail.com",
        ///        "password": "UberHardPassword123!"
        ///     }
        ///
        /// </remarks>
        /// <param name="request">User authentication request model</param>
        /// <returns>JWT</returns>
        /// <response code ="200">Returns the JWT along with it's metadata</response>
        /// <response code ="400">Email or password is incorrect</response>
        [HttpPost]
        [Produces("application/json")]
        public async Task<ActionResult<JwtModel>> Post(UserAuthenticationInformation request)
        {
            var user = await _userManager.GetUser(request.Email, request.Password);
            if (user is null)
                return BadRequest("Email or password incorrect.");

            return Ok(await _tokenProvider.GenerateJwt(user));
        }
    }
}