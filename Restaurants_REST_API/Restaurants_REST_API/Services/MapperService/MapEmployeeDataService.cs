using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.UpdateDataService
{
    public class MapEmployeeDataService
    {
        private readonly GetEmployeeDTO _employeeDetailsDatabase;
        private readonly PutEmployeeDTO _newEmployeeData;
        private Employee employeeUpdatedData;

        public MapEmployeeDataService(GetEmployeeDTO employeeDetailsDatabase, PutEmployeeDTO newEmployeeData)
        {
            _employeeDetailsDatabase = employeeDetailsDatabase;
            _newEmployeeData = newEmployeeData;

            employeeUpdatedData = new Employee();
            employeeUpdatedData.Address = new Address();

        }

        private void UpdatedEmployeeData()
        {
            string newFirstName = _newEmployeeData.FirstName;
            string oldFirstName = _employeeDetailsDatabase.FirstName;

            string newLastName = _newEmployeeData.LastName;
            string oldLastName = _employeeDetailsDatabase.LastName;

            string newPesel = _newEmployeeData.PESEL;
            string oldPesel = _employeeDetailsDatabase.PESEL;

            decimal newSalary = _newEmployeeData.Salary;
            decimal oldSalary = _employeeDetailsDatabase.Salary;

            decimal newBonusSalary = _newEmployeeData.BonusSalary;
            decimal oldBonusSalary = _employeeDetailsDatabase.BonusSalary;

            //setting first name
            if (newFirstName.Equals(oldFirstName))
            {
                employeeUpdatedData.FirstName = oldFirstName;
            }
            else
            {
                employeeUpdatedData.FirstName = newFirstName;
            }

            //setting last name
            if (newLastName.Equals(oldLastName))
            {
                employeeUpdatedData.LastName = oldLastName;
            }
            else
            {
                employeeUpdatedData.LastName = newLastName;
            }

            //setting pesel
            if (newPesel.Equals(oldPesel))
            {
                employeeUpdatedData.PESEL = oldPesel;
            }
            else
            {
                employeeUpdatedData.PESEL = newPesel;
            }

            //setting salary
            if (newSalary == oldSalary)
            {
                employeeUpdatedData.Salary = oldSalary;
            }
            else
            {
                employeeUpdatedData.Salary = newSalary;
            }

            //setting bonus salary
            if (newBonusSalary == oldBonusSalary)
            {
                employeeUpdatedData.BonusSalary = oldBonusSalary;
            }
            else
            {
                employeeUpdatedData.BonusSalary = newBonusSalary;
            }
        }

        private void UpdatedEmployeeAddress()
        {
            string newCityData = _newEmployeeData.Address.City;
            string oldCityData = _employeeDetailsDatabase.Address.City;

            string newStreetData = _newEmployeeData.Address.Street;
            string oldStreetData = _employeeDetailsDatabase.Address.Street;

            string newBuildingNumberData = _newEmployeeData.Address.BuildingNumber;
            string oldBuildingNumberData = _employeeDetailsDatabase.Address.BuildingNumber;

            string? newLocalNumber = _newEmployeeData.Address.LocalNumber;
            string? oldLocalNumber = _employeeDetailsDatabase.Address.LocalNumber;

            //setting city address 
            if (newCityData.Equals(oldCityData))
            {
                employeeUpdatedData.Address.City = oldCityData;
            }
            else
            {
                employeeUpdatedData.Address.City = newCityData;
            }

            //setting street address 
            if (newStreetData.Equals(oldStreetData))
            {
                employeeUpdatedData.Address.Street = oldStreetData;
            }
            else
            {
                employeeUpdatedData.Address.Street = newStreetData;
            }

            //setting building number address 
            if (newBuildingNumberData.Equals(oldBuildingNumberData))
            {
                employeeUpdatedData.Address.BuildingNumber = oldBuildingNumberData;
            }
            else
            {
                employeeUpdatedData.Address.BuildingNumber = newBuildingNumberData;
            }

            //setting local number
            if (newLocalNumber != null && oldLocalNumber != null)
            {
                if (newLocalNumber.Equals(oldLocalNumber))
                {
                    employeeUpdatedData.Address.LocalNumber = oldLocalNumber;
                }
                else
                {
                    employeeUpdatedData.Address.LocalNumber = newLocalNumber;
                }
            }
            else
            {
                employeeUpdatedData.Address.LocalNumber = newLocalNumber;
            }
        }

        public Employee GetEmployeeUpdatedData()
        {
            UpdatedEmployeeData();
            UpdatedEmployeeAddress();

            return employeeUpdatedData;
        }
    }
}
