using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PutDTO;

namespace Restaurants_REST_API.Services.MapperService
{
    public class MapEmployeeCertificatesService
    {
        private readonly GetEmployeeDTO _employeeDetailsDatabase;
        private readonly IEnumerable<PutCertificateDTO> _certificatesData;
        private List<string> updatedCertificateNames = new List<string>();
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
                string oldName = _employeeDetailsDatabase.Certificates.ElementAt(i).Name.ToLower();
                string newName = _certificatesData.ElementAt(i).Name.ToLower();

                if (!oldName.Equals(newName))
                {
                    updatedCertificateNames.Add(newName);
                    updatedCertificatesId.Add(_employeeDetailsDatabase.Certificates.ElementAt(i).IdCertificate);
                }
            }

        }

        public List<string> GetUpdatedCertificateNames()
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
