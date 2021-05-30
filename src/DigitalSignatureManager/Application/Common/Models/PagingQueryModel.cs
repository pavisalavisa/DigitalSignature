using FluentValidation;

namespace Application.Common.Models
{
    public class PagingQueryModel
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;

        public class RegisterUserModelValidator : AbstractValidator<PagingQueryModel>
        {
            public RegisterUserModelValidator()
            {
                RuleFor(x => x.Page)
                    .GreaterThan(0).WithMessage("Pages are 1-indexed and must be greater than 0");

                RuleFor(x => x.Size)
                    .GreaterThan(0).WithMessage("Page size should be greater than 0.");
            }
        }
    }
}