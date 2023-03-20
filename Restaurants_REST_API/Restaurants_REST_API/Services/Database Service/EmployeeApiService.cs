using Restaurants_REST_API.Models;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class EmployeeApiService : IEmployeeApiService
    {
        public Task<IEnumerable<Employee>> GetAllEmployeesAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Employee> GetEmployeeByIdAsync(int empId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> GetAllSupervisorsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetSupervisorByIdAsync(int empId)
        {
            throw new NotImplementedException();
        }

        public Task<Employee> GetOwnerAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Employee>> GetAllEmployeesByRestaurantIdAsync(int restaurantId)
        {
            throw new NotImplementedException();
        }
               
    }
}
