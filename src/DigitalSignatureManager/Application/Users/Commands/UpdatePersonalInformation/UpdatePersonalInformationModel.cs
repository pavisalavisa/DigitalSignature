using Application.Common;
using Application.Common.Contracts;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Users.Commands.UpdatePersonalInformation
{
    public class UpdatePersonalInformationModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganizationName { get; set; }

        public class Validator : AbstractValidator<UpdatePersonalInformationModel>
        {
            public Validator(IApplicationUserManager userManager, IHttpContextAccessor contextAccessor)
            {
                var userId = contextAccessor.GetUserIdFromClaims();
                
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email should not be empty.")
                    .EmailAddress()
                    .MustAsync(async (s, t) => !await userManager.EmailExists(s, userId))
                    .WithMessage("Email address must be unique.");
            }
        }
    }
}