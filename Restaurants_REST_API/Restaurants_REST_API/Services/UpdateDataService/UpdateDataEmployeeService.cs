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
            //setting first name
            if (_newEmployeeData.FirstName.Equals(_employeeDatabase.FirstName))
            {
                _employeeUpdatedData.FirstName = _employeeDatabase.FirstName;
            }
            else
            {
                _employeeUpdatedData.FirstName = _newEmployeeData.FirstName;
            }

            //setting last name
            if (_newEmployeeData.LastName.Equals(_employeeDatabase.LastName))
            {
                _employeeUpdatedData.LastName = _employeeDatabase.LastName;
            }
            else
            {
                _employeeUpdatedData.LastName = _newEmployeeData.LastName;
            }

            //setting pesel
            if (_newEmployeeData.PESEL.Equals(_employeeDatabase.PESEL))
            {
                _employeeUpdatedData.PESEL = _employeeDatabase.PESEL;
            }
            else
            {
                _employeeUpdatedData.PESEL = _newEmployeeData.PESEL;
            }

            //setting pesel
            if (_newEmployeeData.PESEL.Equals(_employeeDatabase.PESEL))
            {
                _employeeUpdatedData.PESEL = _employeeDatabase.PESEL;
            }
            else
            {
                _employeeUpdatedData.PESEL = _newEmployeeData.PESEL;
            }

            //setting salary
            if (_newEmployeeData.Salary == _employeeDatabase.Salary)
            {
                _employeeUpdatedData.Salary = _employeeDatabase.Salary;
            }
            else
            {
                _employeeUpdatedData.Salary = _newEmployeeData.Salary;
            }

            //setting bonus salary
            if (_newEmployeeData.BonusSalary == _employeeDatabase.BonusSalary)
            {
                _employeeUpdatedData.BonusSalary = _employeeDatabase.BonusSalary;
            }
            else
            {
                _employeeUpdatedData.BonusSalary = _newEmployeeData.BonusSalary;
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
