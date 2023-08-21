using Restaurants_REST_API.DTOs.GetDTO;

namespace Restaurants_REST_API.Services.ValidatorService
{
    public class EmployeeTypeValidator
    {
        public static bool isTypesExist(IEnumerable<GetEmployeeTypeDTO>? allTypes)
        {
            if (allTypes == null || allTypes.Count() == 0)
            {
                return false;
            }
            return true;
        }

        public static bool isTypeExistInById(IEnumerable<GetEmployeeTypeDTO>? allTypes, int typeId)
        {
            if (allTypes == null || allTypes.Count() == 0) 
            {
                return false;
            } 

            string? typeNameQuery = allTypes
                .Where(at => at?.IdType == typeId)
                .Select(at => at?.Name)
                .FirstOrDefault();

            return typeNameQuery != null;
        }

        public static bool isTypeExistInByName(IEnumerable<GetEmployeeTypeDTO>? allTypes, string name)
        {
            if (allTypes == null || allTypes.Count() == 0) 
            {
                return false;
            }

            string? typeNameQuery = allTypes
                .Where(at => at?.Name.ToLower() == name.ToLower())
                .Select(at => at?.Name)
                .FirstOrDefault();

            return typeNameQuery != null;
        }
    }
}
