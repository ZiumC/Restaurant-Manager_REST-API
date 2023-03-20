using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Models;

namespace Restaurants_REST_API.Services.Database_Service
{
    public interface IEmployeeApiService
    {
        public Task<IEnumerable<EmployeeDTO>> GetAllEmployeesAsync();
        public Task<Employee?> GetBasicEmployeeDataByIdAsync(int empId);
        public Task<EmployeeDTO> GetDetailedEmployeeDataAsync(Employee employeeData);
        public Task<IEnumerable<Employee>> GetAllSupervisorsAsync();
        public Task<Employee> GetSupervisorByIdAsync(int empId);
        public Task<Employee> GetOwnerAsync();
        public Task<IEnumerable<Employee>> GetAllEmployeesByRestaurantIdAsync(int restaurantId);
    }
}
