using System;

namespace Application.Certificates.Queries
{
    public class CertificateModel
    {
        public int Id { get; set; }
        public string OwnerName { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }
        public bool IsRevoked { get; set; }
    }
}