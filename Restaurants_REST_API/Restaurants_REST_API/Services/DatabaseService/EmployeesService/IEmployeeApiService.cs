using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IEmployeeApiService
    {
        public Task<IEnumerable<GetEmployeeDTO>> GetAllEmployeesAsync();
        public Task<Employee?> GetBasicEmployeeDataByIdAsync(int empId);
        public Task<GetEmployeeDTO> GetDetailedEmployeeDataAsync(Employee employeeData);
        public Task<IEnumerable<GetEmployeeDTO>> GetSupervisorsDetailsAsync(List<int> supervisorsId);
        public Task<IEnumerable<int>?> GetSupervisorsIdAsync();

        /*
         * this method is specially doesn't implemented, because implementation would be very similar to
         * implementation of GetSupervisorsDetailsAsync(List<int>)
         */
        //public Task<Employee> GetSupervisorDetailsByIdAsync(int id);

        public Task<Employee?> GetOwnerBasicDataAsync();
        public Task<IEnumerable<GetEmployeeDTO>> GetAllEmployeesByRestaurantIdAsync(int restaurantId);
        public Task<IEnumerable<EmployeeType?>> GetAllEmployeeTypesAsync();
        public Task<bool> AddNewEmployeeAsync(PostEmployeeDTO newEmployee, bool certificatesExist);
        public Task<bool> AddNewEmployeeTypeAsync(string name);
        public Task<bool> UpdateExistingEmployeeByIdAsync(int id, Employee updatedEmployeeData);
        public Task<bool> UpdateExistingEmployeeCertificatesByIdAsync(List<PutCertificateDTO> updatedCertificatesData, List<int> updatedCertificatesId);

    }
}
