using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.MapperService
{
    public class MapEmployeeCertificatesService
    {
        private readonly GetEmployeeDTO _employeeDetailsDatabase;
        private readonly PutCertificateDTO _newCertificatesData;
        private readonly int _certificateId;
        private PutCertificateDTO updatedCertificate = new PutCertificateDTO();

        public MapEmployeeCertificatesService(GetEmployeeDTO employeeDetailsDatabase, PutCertificateDTO certificatesData, int certificateId)
        {
            _employeeDetailsDatabase = employeeDetailsDatabase;
            _newCertificatesData = certificatesData;
            _certificateId = certificateId;
        }

        private void UpdateEmployeeCertificates()
        {
            GetCertificateDTO oldCertificateQuery = _employeeDetailsDatabase.Certificates
                .Where(ec => ec.IdCertificate == _certificateId).First();

            string oldName = oldCertificateQuery.Name.ToLower();
            string newName = _newCertificatesData.Name.ToLower();

            DateTime oldExpirationDate = oldCertificateQuery.ExpirationDate.Date;
            DateTime newExpirationDate = _newCertificatesData.ExpirationDate.Date;

            if (oldName.Equals(newName))
            {
                updatedCertificate.Name = oldName;
            }
            else
            {
                updatedCertificate.Name = newName;
            }

            if (oldExpirationDate == newExpirationDate)
            {
                updatedCertificate.ExpirationDate = oldExpirationDate;
            }
            else
            {
                updatedCertificate.ExpirationDate = newExpirationDate;
            }

        }

        public PutCertificateDTO GetUpdatedCertificateNames()
        {
            UpdateEmployeeCertificates();
            return updatedCertificate;
        }
    }
}
