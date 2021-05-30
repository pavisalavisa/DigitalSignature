using System.Threading.Tasks;
using Application.Common.Models;

namespace Application.Users.Commands.RegisterUser
{
    public interface IRegisterUserCommand
    {
        Task<EntityCreatedModel> Execute(RegisterUserModel model); 
    }
}