using System.IO;
using System.Threading.Tasks;

namespace Application.Users.Queries.GetPersonalCertificate
{
    public interface IGetPersonalCertificateQuery
    {
        Task<Stream> Query();
    }
}