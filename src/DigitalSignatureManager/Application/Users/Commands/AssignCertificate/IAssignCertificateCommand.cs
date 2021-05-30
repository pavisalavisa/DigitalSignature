using System.Threading.Tasks;

namespace Application.Users.Commands.AssignCertificate
{
    public interface IAssignCertificateCommand
    {
        public Task Execute(CertificateAssignmentModel model);
    }
}