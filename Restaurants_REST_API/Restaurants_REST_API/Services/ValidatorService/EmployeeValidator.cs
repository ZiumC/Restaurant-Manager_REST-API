using Restaurants_REST_API.DTOs.GetDTOs;
using System.Net;
using System.Text.RegularExpressions;

namespace Restaurants_REST_API.Services.ValidationService
{
    public class EmployeeValidator
    {
        public static bool isEmptyNameOf(GetEmployeeDTO empToCheck)
        {
            if (empToCheck.FirstName.Replace("\\s", "") == "")
            {
                return true;
            }

            if (empToCheck.LastName.Replace("\\s", "") == "")
            {
                return true;
            }
            return false;
        }

        public static bool isCorrectPeselOf(GetEmployeeDTO empToCheck)
        {
            return Regex.IsMatch(empToCheck.PESEL, @"^\d+$");
        }

        public static bool isCorrectSalaryOf(GetEmployeeDTO empToCheck)
        {
            return empToCheck.Salary > 0;
        }

        public static bool isCorrectOwnerFieldOf(GetEmployeeDTO empToCheck)
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

        public static bool isCorrectCertificatesOf(GetEmployeeDTO empToCheck)
        {
            foreach (GetCertificateDTO empCert in empToCheck.Certificates)
            {
                if (empCert.Name.Replace("\\s", "").Equals(""))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool isEmployeeExistIn(IEnumerable<GetEmployeeDTO> allEmployees, GetEmployeeDTO empToCheck)
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
