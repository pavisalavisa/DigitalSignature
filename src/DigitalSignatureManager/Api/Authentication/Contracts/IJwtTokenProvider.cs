using System.Threading.Tasks;
using Domain.Common;
#pragma warning disable 1591

namespace Api.Authentication.Contracts
{
    public interface IJwtTokenProvider
    {
        Task<JwtModel> GenerateJwt(ApplicationUser user);
    }
}