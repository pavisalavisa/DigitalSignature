using System.Threading.Tasks;

namespace Application.Users.Commands.UpdateUser
{
    public interface IUpdateUserCommand
    {
        Task Execute(UpdateUserModel model, int id);
    }
}