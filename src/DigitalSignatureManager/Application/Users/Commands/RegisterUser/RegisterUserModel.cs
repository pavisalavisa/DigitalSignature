using System.Linq;
using Application.Common.Contracts;
using FluentValidation;

namespace Application.Users.Commands.RegisterUser
{
    public class RegisterUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string TeamName { get; set; }
        public string Country { get; set; }

        public class RegisterUserModelValidator : AbstractValidator<RegisterUserModel>
        {
            public RegisterUserModelValidator(IApplicationUserManager userManager)
            {
                RuleFor(x => x.Email)
                    .NotEmpty().WithMessage("Email should not be empty.")
                    .EmailAddress()
                    .MustAsync(async (s, t) => !await userManager.EmailExists(s))
                    .WithMessage("Email address must be unique.");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password should not be empty.")
                    .Must(x => x.Any(char.IsDigit)).WithMessage("Password must contain at least 1 digit.")
                    .Must(x => x.Any(char.IsUpper)).WithMessage("Password must contain at least 1 uppercase character.")
                    .Must(x => x.Any(char.IsLower)).WithMessage("Password must contain at least 1 lowercase character.");

                RuleFor(x => x.TeamName)
                    .NotEmpty()
                    .MaximumLength(128);
                
                RuleFor(x => x.Country)
                    .NotEmpty()
                    .MaximumLength(128);
            }
        }
    }
}