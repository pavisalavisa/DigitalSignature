using FluentValidation;

namespace Application.Users.Commands.UpdateUser
{
    public class UpdateUserModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrganizationName { get; set; }

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