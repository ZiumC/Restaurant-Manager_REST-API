using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Models;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IEmployeeApiService
    {
        public Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();
        public Task<Employee?> GetBasicEmployeeDataByIdAsync(int empId);
        public Task<EmployeeDTO> GetDetailedEmployeeDataAsync(Employee employeeData);
        public Task<IEnumerable<EmployeeDTO>> GetSupervisorsDetailsAsync(List<int> supervisorsId);
        public Task<IEnumerable<int>?> GetSupervisorsIdAsync();

        /*
         * this method is specially doesn't implemented, because implementation would be very similar to
         * implementation of GetSupervisorsDetailsAsync(List<int>)
         */
        //public Task<Employee> GetSupervisorDetailsByIdAsync(int id);

        public Task<Employee?> GetOwnerBasicDataAsync();
        public Task<IEnumerable<EmployeeDTO>> GetAllEmployeesByRestaurantIdAsync(int restaurantId);
    }
}
