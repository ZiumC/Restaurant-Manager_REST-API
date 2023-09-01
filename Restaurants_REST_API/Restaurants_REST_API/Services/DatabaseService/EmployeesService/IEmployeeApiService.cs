using Restaurants_REST_API.DAOs;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IEmployeeApiService
    {
        public Task<IEnumerable<GetEmployeeDTO>?> GetAllEmployeesAsync();
        public Task<Employee?> GetEmployeeSimpleDataByIdAsync(int empId);
        public Task<GetEmployeeDTO?> GetEmployeeDetailsByEmpIdAsync(int empId);
        public Task<IEnumerable<GetEmployeeDTO>?> GetAllEmployeesDetailsByTypeIdAsync(int typeId);
        public Task<GetEmployeeDTO?> GetEmployeeDetailsByTypeIdAsync(int typeId);
        public Task<Employee?> GetEmployeeSimpleDataByPeselAsync(string pesel);
        public Task<IEnumerable<GetEmployeeDTO>?> GetAllEmployeesDetailsByRestaurantIdAsync(int restaurantId);
        public Task<bool> AddNewEmployeeAsync(EmployeeDAO newEmployee, string ownerStatus);
        public Task<bool> AddNewEmployeeCertificatesAsync(int empId, IEnumerable<CertificateDAO> empCertificatesData);
        public Task<bool> UpdateEmployeeDataByIdAsync(int empId, EmployeeDAO empData);
        public Task<bool> UpdateEmployeeCertificateByIdAsync(int certificateId, CertificateDAO empCertificateData);
        public Task<bool> DeleteEmployeeDataByIdAsync(int empId, GetEmployeeDTO empData);
        public Task<bool> DeleteEmployeeCertificateAsync(int empId, GetCertificateDTO empCertificateData);

    }
}
