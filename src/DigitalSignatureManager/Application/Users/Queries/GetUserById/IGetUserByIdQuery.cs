using System.Threading.Tasks;

namespace Application.Users.Queries.GetUserById
{
    public interface IGetUserByIdQuery
    {
        Task<UserModel> Query(int id);
    }
}