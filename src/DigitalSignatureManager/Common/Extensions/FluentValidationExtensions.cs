using System;
using Common.Models;
using FluentValidation;

namespace Common.Extensions
{
    public static class FluentValidationExtensions
    {
        public static IRuleBuilder<T, TP> WithCodedErrorMessage<T, TP>(this IRuleBuilderOptions<T, TP> builder, CodedError codedError) =>
            builder.WithErrorCode(codedError.ErrorCode).WithMessage(codedError.ErrorMessage);

        public static IRuleBuilderOptions<T, string> IsB64String<T>(this IRuleBuilder<T, string> builder) => builder.Must(IsB64String);

        private static bool IsB64String(string input)
        {
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Convert.FromBase64String(input);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}