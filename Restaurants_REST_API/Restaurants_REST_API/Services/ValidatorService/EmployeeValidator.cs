using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using System.Net;
using System.Text.RegularExpressions;

namespace Restaurants_REST_API.Services.ValidationService
{
    public class EmployeeValidator
    {
        public static bool isCorrectPeselOf(string peselToCheck)
        {
            if (peselToCheck == null)
            {
                return true;
            }

            return Regex.IsMatch(peselToCheck, @"^\d+$");
        }

        public static bool isCorrectSalaryOf(decimal? salaryToCheck)
        {
            if (salaryToCheck == null)
            {
                return false;
            }

            return salaryToCheck > 0;
        }


        public static bool isEmployeeExistIn(IEnumerable<GetEmployeeDTO> allEmployees, EmployeeDTO empToCheck)
        {
            List<bool> newEmpEquals = new List<bool>();
            foreach (GetEmployeeDTO emp in allEmployees)
            {
                bool empExist = true;
                if (!emp.FirstName.Equals(empToCheck.FirstName))
                {
                    empExist = false;
                }

                if (!emp.LastName.Equals(empToCheck.LastName))
                {
                    empExist = false;
                }

                if (!emp.PESEL.Equals(empToCheck.PESEL))
                {
                    empExist = false;
                }

                if (!emp.IsOwner.Equals(emp.IsOwner))
                {
                    empExist = false;
                }

                if (!emp.Address.City.Equals(empToCheck.Address.City))
                {
                    empExist = false;
                }

                if (!emp.Address.Street.Equals(empToCheck.Address.Street))
                {
                    empExist = false;
                }

                if (!emp.Address.BuildingNumber.Equals(empToCheck.Address.BuildingNumber))
                {
                    empExist = false;
                }

                if (!emp.Address.BuildingNumber.Equals(empToCheck.Address.BuildingNumber))
                {
                    empExist = false;
                }

                newEmpEquals.Add(empExist);
            }

            return newEmpEquals.Contains(true);
        }
    }
}
