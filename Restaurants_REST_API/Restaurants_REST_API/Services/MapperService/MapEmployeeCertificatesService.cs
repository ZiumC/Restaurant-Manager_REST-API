using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PutDTO;

namespace Restaurants_REST_API.Services.MapperService
{
    public class MapEmployeeCertificatesService
    {
        private readonly GetEmployeeDTO _employeeDetailsDatabase;
        private readonly IEnumerable<PutCertificateDTO> _certificatesData;
        private List<PutCertificateDTO> updatedCertificateNames = new List<PutCertificateDTO>();
        private List<int> updatedCertificatesId = new List<int>();

        public MapEmployeeCertificatesService(GetEmployeeDTO employeeDetailsDatabase, IEnumerable<PutCertificateDTO> certificatesData)
        {
            _employeeDetailsDatabase = employeeDetailsDatabase;
            _certificatesData = certificatesData;
        }

        private void UpdateEmployeeCertificates()
        {

            if (_employeeDetailsDatabase.Certificates == null)
            {
                throw new Exception("Employee doesn't have any certificates");
            }

            for (int i = 0; i < _employeeDetailsDatabase.Certificates.Count(); i++)
            {
                var oldEmpCert = _employeeDetailsDatabase.Certificates.ElementAt(i);

                string oldName = oldEmpCert.Name.ToLower();
                string newName = _certificatesData.ElementAt(i).Name.ToLower();

                DateTime oldExpirationDate = oldEmpCert.ExpirationDate.Date;
                DateTime newExpirationDate = _certificatesData.ElementAt(i).ExpirationDate.Date;

                if (!oldName.Equals(newName) || oldExpirationDate != newExpirationDate)
                {
                    updatedCertificateNames.Add(new PutCertificateDTO { Name = newName, ExpirationDate = newExpirationDate });
                    updatedCertificatesId.Add(_employeeDetailsDatabase.Certificates.ElementAt(i).IdCertificate);
                }
            }

        }

        public List<PutCertificateDTO> GetUpdatedCertificateNames()
        {
            UpdateEmployeeCertificates();
            return updatedCertificateNames;
        }

        public List<int> GetUpdatedCertificatesId()
        {
            return updatedCertificatesId;
        }
    }
}
