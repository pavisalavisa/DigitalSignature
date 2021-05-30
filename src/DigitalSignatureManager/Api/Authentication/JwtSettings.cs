#pragma warning disable 1591
namespace Api.Authentication
{
    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int ExpirationInMinutes { get; set; }
    }
}