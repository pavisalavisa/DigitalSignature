using System.Threading.Tasks;

namespace Application.Users.Queries.GetPersonalInformation
{
    public interface IGetPersonalInformationQuery
    {
        Task<PersonalInformationModel> Query();
    }
}