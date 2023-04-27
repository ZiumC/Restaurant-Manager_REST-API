using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.DTOs.PostDTOs;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.ValidationService;

namespace Restaurants_REST_API.Services.UpdateDataService
{
    public class UpdateDataEmployeeService
    {
        private readonly Employee _employeeDatabase;
        private readonly EmployeeDTO _newEmployeeData;
        private readonly bool _certificatesExist;
        private EmployeeDTO _employeeUpdatedData;

        public UpdateDataEmployeeService(Employee employeeDatabase, EmployeeDTO newEmployeeData, bool certificateExists)
        {
            _employeeDatabase = employeeDatabase;
            _newEmployeeData = newEmployeeData;
            _certificatesExist = certificateExists;

            _employeeUpdatedData = new EmployeeDTO();
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
                _employeeUpdatedData.FirstName = oldFirstName;
            }
            else
            {
                _employeeUpdatedData.FirstName = newFirstName;
            }

            //setting last name
            if (newLastName.Equals(oldLastName))
            {
                _employeeUpdatedData.LastName = oldLastName;
            }
            else
            {
                _employeeUpdatedData.LastName = newLastName;
            }

            //setting pesel
            if (newPesel.Equals(oldPesel))
            {
                _employeeUpdatedData.PESEL = oldPesel;
            }
            else
            {
                _employeeUpdatedData.PESEL = newPesel;
            }

            //setting salary
            if (newSalary == oldSalary)
            {
                _employeeUpdatedData.Salary = oldSalary;
            }
            else
            {
                _employeeUpdatedData.Salary = newSalary;
            }

            //setting bonus salary
            if (newBonusSalary == oldBonusSalary)
            {
                _employeeUpdatedData.BonusSalary = oldBonusSalary;
            }
            else
            {
                _employeeUpdatedData.BonusSalary = newBonusSalary;
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
                _employeeUpdatedData.Address.City = oldCityData;
            }
            else
            {
                _employeeUpdatedData.Address.City = newCityData;
            }

            //setting street address 
            if (newStreetData.Equals(oldStreetData))
            {
                _employeeUpdatedData.Address.Street = oldStreetData;
            }
            else
            {
                _employeeUpdatedData.Address.Street = newStreetData;
            }

            //setting building number address 
            if (newBuildingNumberData.Equals(oldBuildingNumberData))
            {
                _employeeUpdatedData.Address.BuildingNumber = oldBuildingNumberData;
            }
            else
            {
                _employeeUpdatedData.Address.BuildingNumber = newBuildingNumberData;
            }

            //setting local number
            if (newLocalNumber != null && oldLocalNumber != null)
            {
                if (newLocalNumber.Equals(oldLocalNumber))
                {
                    _employeeUpdatedData.Address.LocalNumber = oldLocalNumber;
                }
                else
                {
                    _employeeUpdatedData.Address.LocalNumber = newLocalNumber;
                }
            }
            else
            {
                _employeeUpdatedData.Address.LocalNumber = newLocalNumber;
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

            return _employeeUpdatedData;
        }
    }
}
