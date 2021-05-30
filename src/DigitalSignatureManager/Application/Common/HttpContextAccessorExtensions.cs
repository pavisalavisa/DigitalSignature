using System;
using System.Linq;
using System.Security.Claims;
using Application.Common.ErrorManagement.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Application.Common
{
    public static class HttpContextAccessorExtensions
    {
        public static int GetUserIdFromClaims(this IHttpContextAccessor contextAccessor)
        {
            var userIdClaim = contextAccessor.HttpContext?.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null)
                throw new AuthenticationException("Authenticated user does not have an id claim. Login and try again.");

            var userId = Convert.ToInt32(userIdClaim.Value);

            return userId;
        }
    }
}