using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;

namespace Application.Common.ErrorManagement.Exceptions
{
    public class BusinessException : Exception
    {
        private const char ErrorsSeparator = ';';

        private readonly IList<CodedError> _codedError;

        public BusinessException(CodedError codedError) : base(codedError.ErrorMessage)
        {
            _codedError = new List<CodedError> {codedError};
        }

        public BusinessException(CodedError codedError, Exception innerException) : base(codedError.ErrorMessage, innerException)
        {
            _codedError = new List<CodedError> {codedError};
        }

        public BusinessException(params CodedError[] codedErrors) : base(string.Join(ErrorsSeparator, codedErrors.ToList()))
        {
            _codedError = codedErrors.ToList();
        }

        public BusinessException(Exception innerException, params CodedError[] codedErrors) : base(string.Join(ErrorsSeparator, codedErrors.ToList()),
            innerException)
        {
            _codedError = codedErrors.ToList();
        }

        public IEnumerable<CodedError> GetCodedErrors() => _codedError;

        public string GetDetails() => string.Join(ErrorsSeparator, _codedError.Select(x => $"{x.Details} [code: {x.ErrorCode}]"));
    }
}