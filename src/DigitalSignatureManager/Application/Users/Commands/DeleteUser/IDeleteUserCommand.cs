using System.Threading.Tasks;

namespace Application.Users.Commands.DeleteUser
{
    public interface IDeleteUserCommand
    {
        Task Execute(int id);
    }
}