using System.Threading.Tasks;
using Application.Common.Models;
using Application.Users.Commands.AssignCertificate;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.RegisterUser;
using Application.Users.Commands.UpdatePersonalInformation;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetAllUsers;
using Application.Users.Queries.GetPersonalCertificate;
using Application.Users.Queries.GetPersonalInformation;
using Application.Users.Queries.GetUserById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#pragma warning disable 1573

namespace Api.Controllers
{
    /// <summary>
    /// User related actions. 
    /// </summary>
    public class UsersController : BaseController
    {
        /// <summary>
        /// Creates the user with provided email and password.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Users/Registration
        ///     {
        ///        "email": "jon.doe@gmail.com",
        ///        "password": "UberHardPassword123!"
        ///     }
        ///
        /// </remarks>
        /// <param name="model">User registration request model</param>
        /// <returns>Id of the newly created user</returns>
        /// <response code ="201">User was successfully created</response>
        /// <response code ="400">Validation error happened</response>
        [HttpPost]
        [Route("Registration")]
        public async Task<ActionResult<EntityCreatedModel>> PostRegistration(RegisterUserModel model,
            [FromServices] IRegisterUserCommand command)
        {
            // Route omitted for the sake of simplicity.
            return Created(string.Empty, await command.Execute(model));
        }

        /// <summary>
        /// Gets the list of all users with pagination
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Users?page=2&amp;size=20
        ///
        /// </remarks>
        /// <param name="pagingModel">Pagination options</param>
        /// <returns>Returns list of ids and names of users</returns>
        /// <response code ="200">Users were successfully fetched.</response>
        /// <response code ="401">Only administrators may perform this action</response>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PagingResultModel<IdNameModel>>> Get([FromQuery] PagingQueryModel pagingModel,
            [FromServices] IGetAllUsersQuery query)
        {
            return Ok(await query.Query(pagingModel));
        }

        /// <summary>
        /// Gets the user by id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Users/123
        ///
        /// </remarks>
        /// <param name="id">User id</param>
        /// <returns>Returns the user with provided id</returns>
        /// <response code ="200">User was successfully fetched.</response>
        /// <response code ="404">User does not exist</response>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public async Task<ActionResult<UserModel>> Get([FromRoute] int id, [FromServices] IGetUserByIdQuery query)
        {
            var user = await query.Query(id);
            if (user is null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Edits the user with provided id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Users/123
        ///     {
        ///        "email": "jon.doe@gmail.com"
        ///     }
        /// 
        ///
        /// </remarks>
        /// <param name="id">User id</param>
        /// <param name="model">User update model</param>
        /// <returns>Ok empty json body</returns>
        /// <response code ="200">User was successfully updated.</response>
        /// <response code ="400">Validation error happened</response>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public async Task<ActionResult> Put([FromRoute] int id, [FromBody] UpdateUserModel model,
            [FromServices] IUpdateUserCommand command)
        {
            await command.Execute(model, id);
            return Ok();
        }

        /// <summary>
        /// Deletes the user with the provided id
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/Users/123
        /// 
        ///
        /// </remarks>
        /// <param name="id">User id</param>
        /// <returns>No content with empty json body</returns>
        /// <response code ="204">User was successfully deleted.</response>
        /// <response code ="400">Validation error happened</response>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id:int}")]
        public async Task<ActionResult> Delete([FromRoute] int id, [FromServices] IDeleteUserCommand command)
        {
            await command.Execute(id);

            return NoContent();
        }

        /// <summary>
        /// Assigns the provided certificate to the authenticated user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Users/Certificate
        ///     {
        ///        "b64Certificate": "Ik15Q2VydGlmaWNhdGUiIA0K",
        ///        "certificatePassword": "UberHardPassword123!"
        ///     }
        ///
        /// </remarks>
        /// <param name="model">Certificate request model</param>
        /// <returns>Ok with empty json body</returns>
        /// <response code ="200">Certificate was successfully assigned</response>
        /// <response code ="400">Validation error happened</response>
        /// <response code ="401">Authentication error happened</response>
        [HttpPost]
        [Authorize(Roles = "Admin, RegularUser")]
        [Route("Certificate")]
        public async Task<ActionResult> PostCertificate(CertificateAssignmentModel model,
            [FromServices] IAssignCertificateCommand command)
        {
            await command.Execute(model);

            return Ok();
        }

        /// <summary>
        /// Gets the personal certificate of the authenticated user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/Users/Certificate
        /// 
        /// </remarks>
        /// <returns>Ok with certificate file result</returns>
        /// <response code ="200">Certificate was successfully exported</response>
        /// <response code ="400">Validation error happened</response>
        /// <response code ="401">Authentication error happened</response>
        /// <response code ="404">Certificate could not be found</response>
        [HttpGet]
        [Authorize(Roles = "Admin, RegularUser")]
        [Route("Certificate")]
        public async Task<ActionResult> PostCertificate([FromServices] IGetPersonalCertificateQuery query)
        {
            var result = await query.Query();

            return new FileStreamResult(result, "application/pkcs-12");;
        }
        
        /// <summary>
        /// Get the personal information of currently authenticated user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/Users/PersonalInformation
        ///
        /// </remarks>
        /// <returns>Ok with personal information json body</returns>
        /// <response code ="200">Personal information was successfully fetched</response>
        /// <response code ="400">Validation error happened</response>
        /// <response code ="401">Authentication error happened</response>
        /// <response code ="404">Personal information does not exist</response>
        [HttpGet]
        [Authorize(Roles = "Admin, RegularUser")]
        [Route("PersonalInformation")]
        public async Task<ActionResult> GetPersonalInformation([FromServices] IGetPersonalInformationQuery query)
        {
            var personalInformation = await query.Query();
            if (personalInformation is null)
            {
                return new NotFoundResult();
            }

            return Ok(personalInformation);
        }

        /// <summary>
        /// Updates the personal information of currently authenticated user
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/Users/PersonalInformation
        ///     {
        ///        "email": "jon.doe@gmail.com",
        ///        "firstName": "Jon",
        ///        "lastName": "Doe",
        ///        "organizationName": "Google"
        ///     }
        ///
        /// </remarks>
        /// <returns>No content</returns>
        /// <response code ="204">Personal information was successfully updated</response>
        /// <response code ="400">Validation error happened</response>
        /// <response code ="401">Authentication error happened</response>
        /// <response code ="404">Personal information does not exist</response>
        [HttpPut]
        [Authorize(Roles = "Admin, RegularUser")]
        [Route("PersonalInformation")]
        public async Task<ActionResult> PostCertificate(UpdatePersonalInformationModel model,
            [FromServices] IUpdatePersonalInformationCommand command)
        {
            await command.Execute(model);

            return NoContent();
        }
    }
}