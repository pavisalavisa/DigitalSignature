using System;

namespace Application.Common.ErrorManagement.Exceptions
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message)
        {
        }
    }
}