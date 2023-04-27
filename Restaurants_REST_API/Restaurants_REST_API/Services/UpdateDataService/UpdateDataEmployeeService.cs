using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.DTOs.PostDTOs;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.ValidationService;

namespace Restaurants_REST_API.Services.UpdateDataService
{
    public class UpdateDataEmployeeService
    {
        private readonly Employee _employeeDatabase;
        private readonly PostEmployeeDTO _newEmployeeData;

        public UpdateDataEmployeeService(Employee employeeDatabase, PostEmployeeDTO newEmployeeData)
        {
            _employeeDatabase = employeeDatabase;
            _newEmployeeData = newEmployeeData;

        }
    }
}
