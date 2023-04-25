using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.ValidatorService
{
    public class EmployeeTypeValidator
    {
        public static bool isCorrectEmployeeTypeOf(int typeToCheck, IEnumerable<EmployeeType?> allTypes)
        {
            foreach (EmployeeType? empType in allTypes)
            {
                if (typeToCheck == empType?.IdType)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isEmployeeAlreadyHasTypeIn(RestaurantDTO restaurantDTO, int empId, string typeName)
        {

            if (restaurantDTO.RestaurantWorkers == null || restaurantDTO.RestaurantWorkers.Count() == 0)
            {
                return false;
            }

            foreach (RestaurantWorkersDTO worker in restaurantDTO.RestaurantWorkers)
            {
                if (worker.IdEmployee == empId && worker.EmployeeType.ToLower().Equals(typeName.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool isTypeExistIn(IEnumerable<EmployeeType?> allTypes, string name)
        {
            if (allTypes == null)
            {
                return false;
            }

            foreach (EmployeeType? empType in allTypes)
            {
                if (name.ToLower().Equals(empType?.Name.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
