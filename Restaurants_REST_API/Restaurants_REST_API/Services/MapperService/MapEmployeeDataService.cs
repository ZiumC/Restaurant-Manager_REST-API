using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.UpdateDataService
{
    public class MapEmployeeDataService
    {
        private readonly Employee _employeeDatabase;
        private readonly EmployeeDTO _newEmployeeData;
        private readonly bool _certificatesExist;
        private EmployeeDTO employeeUpdatedData;

        public MapEmployeeDataService(Employee employeeDatabase, EmployeeDTO newEmployeeData, bool certificateExists)
        {
            _employeeDatabase = employeeDatabase;
            _newEmployeeData = newEmployeeData;
            _certificatesExist = certificateExists;

            employeeUpdatedData = new EmployeeDTO();
        }

        private void UpdatedEmployeeData()
        {
            string newFirstName = _newEmployeeData.FirstName;
            string oldFirstName = _employeeDatabase.FirstName;

            string newLastName = _newEmployeeData.LastName;
            string oldLastName = _employeeDatabase.LastName;

            string newPesel = _newEmployeeData.PESEL;
            string oldPesel = _employeeDatabase.PESEL;

            decimal newSalary = _newEmployeeData.Salary;
            decimal oldSalary = _employeeDatabase.Salary;

            decimal newBonusSalary = _newEmployeeData.BonusSalary;
            decimal oldBonusSalary = _employeeDatabase.BonusSalary;

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
            string oldCityData = _employeeDatabase.Address.City;

            string newStreetData = _newEmployeeData.Address.Street;
            string oldStreetData = _employeeDatabase.Address.Street;

            string newBuildingNumberData = _newEmployeeData.Address.BuildingNumber;
            string oldBuildingNumberData = _employeeDatabase.Address.BuildingNumber;

            string? newLocalNumber = _newEmployeeData.Address.LocalNumber;
            string? oldLocalNumber = _employeeDatabase.Address.LocalNumber;

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

        private void UpdatedEmployeeCertificates()
        {

        }

        public EmployeeDTO GetEmployeeUpdatedData()
        {
            UpdatedEmployeeData();
            UpdatedEmployeeAddress();

            if (_certificatesExist)
            {
                UpdatedEmployeeCertificates();
            }

            return employeeUpdatedData;
        }
    }
}
