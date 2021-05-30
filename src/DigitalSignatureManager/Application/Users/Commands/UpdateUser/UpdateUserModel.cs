using FluentValidation;

namespace Application.Users.Commands.UpdateUser
{
    public class UpdateUserModel
    {
        public string Email { get; set; }

        public class UpdateUserModelValidator : AbstractValidator<UpdateUserModel>
        {
            public UpdateUserModelValidator()
            {
                RuleFor(x => x.Email)
                    .EmailAddress();
            }
        }
    }
}