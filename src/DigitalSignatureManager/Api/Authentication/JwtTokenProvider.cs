using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Authentication.Contracts;
using Application.Common.Contracts;
using Domain.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

#pragma warning disable 1591
// Justification: Used for Swagger XML generation

namespace Api.Authentication
{
    public class JwtTokenProvider : IJwtTokenProvider
    {
        private readonly IApplicationUserManager _userManager;
        private readonly JwtSettings _jwtSettings;

        public JwtTokenProvider(IApplicationUserManager userManager, IOptionsSnapshot<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<JwtModel> GenerateJwt(ApplicationUser user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            var roles = await _userManager.GetUserRoles(user);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(ClaimTypes.Name, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r.ToString()));
            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);

            var token = new JwtSecurityToken(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return new JwtModel
            {
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                ExpiresAt = expires,
                Jwt = jwt
            };
        }
    }
}