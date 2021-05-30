using System;

namespace Common.Models
{
    public record CodedError(string ErrorCode, string ErrorMessage, string Details = null);
}