using System.Threading.Tasks;

namespace Application.Users.Commands.UpdatePersonalInformation
{
    public interface IUpdatePersonalInformationCommand
    {
        Task Execute(UpdatePersonalInformationModel model);
    }
}