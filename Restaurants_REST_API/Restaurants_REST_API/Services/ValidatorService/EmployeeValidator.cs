using Restaurants_REST_API.DTOs;
using System.Net;
using System.Text.RegularExpressions;

namespace Restaurants_REST_API.Services.ValidationService
{
    public class EmployeeValidator
    {
        public static bool isEmptyNameOf(EmployeeDTO empToCheck)
        {
            if (empToCheck.FirstName.Replace("\\s", "") == "")
            {
                return false;
            }

            if (empToCheck.Surname.Replace("\\s", "") == "")
            {
                return false;
            }
            return true;
        }

        public static bool isCorrectPeselOf(EmployeeDTO empToCheck)
        {
            return Regex.IsMatch(empToCheck.PESEL, @"^\d+$");
        }

        public static bool isCorrectSalaryOf(EmployeeDTO empToCheck)
        {
            return empToCheck.Salary > 0;
        }

        public static bool isCorrectOwnerFieldOf(EmployeeDTO empToCheck)
        {
            if (empToCheck.IsOwner.Replace("\\s", "") == "") 
            {
                return false;
            }

            string isOwner = empToCheck.IsOwner.ToLower();
            if (isOwner.Equals("y") || isOwner.Equals("n")) 
            {
                return true;
            }

            return false;
        }

        public static bool isCorrectAddressOf(AddressDTO empAddress)
        {
            if (empAddress.City.Replace("\\s", "") == "")
            {
                return false;
            }

            if (empAddress.Street.Replace("\\s", "") == "")
            {
                return false;
            }

            if (empAddress.BuildingNumber.Replace("\\s", "") == "")
            {
                return false;
            }

            return true;
        }
    }
}
