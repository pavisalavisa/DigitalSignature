using System.Threading.Tasks;
using Domain.Enums;

namespace Application.Common.Contracts
{
    public interface IApplicationRoleManager
    {
        Task CreateRoleAsync(Roles role);
    }
}