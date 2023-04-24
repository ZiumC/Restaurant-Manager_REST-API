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
                return true;
            }

            if (empToCheck.Surname.Replace("\\s", "") == "")
            {
                return true;
            }
            return false;
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

        public static bool isCorrectCertificatesOf(EmployeeDTO empToCheck)
        {
            foreach (CertificateDTO empCert in empToCheck.Certificates)
            {
                if (empCert.Name.Replace("\\s", "").Equals(""))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool isEmployeeExistIn(IEnumerable<EmployeeDTO> allEmployees, EmployeeDTO empToCheck)
        {
            List<bool> newEmpEquals = new List<bool>();
            foreach (EmployeeDTO emp in allEmployees)
            {
                bool empExist = true;
                if (!emp.FirstName.Equals(empToCheck.FirstName))
                {
                    empExist = false;
                }

                if (!emp.Surname.Equals(empToCheck.Surname))
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
