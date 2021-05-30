using Common.Models;
using FluentValidation;

namespace Common.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilderOptions<T, TP> WithCodedErrorMessage<T, TP>(this IRuleBuilderOptions<T, TP> builder, CodedError codedError) =>
            builder.WithErrorCode(codedError.ErrorCode).WithMessage(codedError.ErrorMessage);
    }
}