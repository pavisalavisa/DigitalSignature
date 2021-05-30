using System;
#pragma warning disable 1591

namespace Api.Authentication
{
    public class JwtModel
    {
        public string Jwt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}