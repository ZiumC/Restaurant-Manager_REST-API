﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.Models;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.ValidationService;
using Restaurants_REST_API.Services.ValidatorService;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Services.UpdateDataService;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Services.MapperService;
using Restaurants_REST_API.DTOs.GetDTO;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/menage/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeApiService _employeeApiService;
        private readonly IRestaurantApiService _restaurantsApiService;
        private readonly decimal _basicBonus = 150;
        private readonly int _idOwnerType = 1;

        public EmployeesController(IEmployeeApiService employeeApiService, IRestaurantApiService restaurantsApiService)
        {
            _employeeApiService = employeeApiService;
            _restaurantsApiService = restaurantsApiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeApiService.GetAllEmployeesAsync();

            if (employees == null)
            {
                return NotFound("Employees not found");
            }

            return Ok(employees);
        }

        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetEmployeeBy(int id)
        {
            if (id < 0)
            {
                return BadRequest($"Incorrect id, expected id grater than 0 but got {id}");
            }

            var employee = await _employeeApiService.GetBasicEmployeeDataByIdAsync(id);

            if (employee == null)
            {
                return NotFound($"Employee {id} not found");
            }

            return Ok(await _employeeApiService.GetDetailedEmployeeDataAsync(employee));
        }

        [HttpGet]
        [Route("supervisors")]
        public async Task<IActionResult> GetSupervisors()
        {
            var supervisorsId = await _employeeApiService.GetSupervisorsIdAsync();

            if (supervisorsId == null)
            {
                return NotFound("No supervisors found");
            }

            return Ok(await _employeeApiService.GetSupervisorsDetailsAsync((List<int>)supervisorsId));
        }

        /*
         * This method uses GetSupervisorsDetailsAsync(List<int>) because implementation of 
         * GetSupervisorDetailsByIdAsync(int id) in interface IEmployeeApiService would very similar!
         */
        [HttpGet]
        [Route("supervisor/id")]
        public async Task<IActionResult> GetSupervisors(int id)
        {
            if (id < 0)
            {
                return BadRequest($"Incorrect id, expected id grater than 0 but got {id}");
            }

            var supervisorsIdList = await _employeeApiService.GetSupervisorsIdAsync();

            if (supervisorsIdList == null)
            {
                return NotFound($"Supervisors not exist");
            }

            if (!supervisorsIdList.Contains(id))
            {
                return NotFound($"Supervisor {id} not exist");
            }

            return Ok(await _employeeApiService.GetSupervisorsDetailsAsync(new List<int> { id }));
        }

        [HttpGet]
        [Route("owner")]
        public async Task<IActionResult> GetOwnerDerails()
        {
            Employee? ownerBasicData = await _employeeApiService.GetOwnerBasicDataAsync();

            if (ownerBasicData == null)
            {
                return NotFound($"Owner not found");
            }

            return Ok(await _employeeApiService.GetDetailedEmployeeDataAsync(ownerBasicData));
        }

        [HttpGet]
        [Route("by-restaurant/id")]
        public async Task<IActionResult> GetEmployeeByRestaurant(int id)
        {
            if (id < 0)
            {
                return BadRequest($"Incorrect id, expected id grater than 0 but got {id}");
            }

            Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(id);

            if (restaurant == null)
            {
                return NotFound($"Restaurant not found");
            }

            var employeesByRestaurant = await _employeeApiService.GetAllEmployeesByRestaurantIdAsync(id);

            if (employeesByRestaurant.Count() == 0)
            {
                return NotFound($"Employees not found");
            }

            return Ok(employeesByRestaurant);
        }

        [HttpGet]
        [Route("types")]
        public async Task<IActionResult> GetAllEmployeeTypes()
        {
            var types = await _employeeApiService.GetAllEmployeeTypesAsync();
            if (types == null)
            {
                return NotFound("Employee types not found");
            }

            return Ok(types);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmployee(PostEmployeeDTO? newEmployee)
        {
            //validating new employee
            if (newEmployee == null)
            {
                return BadRequest("Employee should be specified");
            }

            if (GeneralValidator.isEmptyNameOf(newEmployee.FirstName) || GeneralValidator.isEmptyNameOf(newEmployee.LastName))
            {
                return BadRequest("First or last name can't be empty");
            }

            if (!EmployeeValidator.isCorrectPeselOf(newEmployee.PESEL))
            {
                return BadRequest("PESEL isn't correct");
            }

            if (!EmployeeValidator.isCorrectSalaryOf(newEmployee.Salary))
            {
                return BadRequest("Salary can't be less or equal 0");
            }

            if (GeneralValidator.isEmptyNameOf(newEmployee.Address.City))
            {
                return BadRequest("City can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(newEmployee.Address.Street))
            {
                return BadRequest("Street can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(newEmployee.Address.BuildingNumber))
            {
                return BadRequest("Building number can't be empty");
            }

            bool certificatesExist = false;
            if (newEmployee.Certificates != null && newEmployee.Certificates.Count() > 0)
            {
                certificatesExist = true;
                foreach (var certificate in newEmployee.Certificates)
                {
                    if (GeneralValidator.isEmptyNameOf(certificate.Name))
                    {
                        return BadRequest("One or more certificates is invalid");
                    }
                }
            }

            //need to check if emp exist in db
            var allEmployees = await _employeeApiService.GetAllEmployeesAsync();
            if (EmployeeValidator.isEmployeeExistIn(allEmployees, newEmployee))
            {
                return BadRequest("Employee already exist");
            }

            //checking if newEmp has bonus sal less than minimum value
            if (newEmployee.BonusSalary < _basicBonus)
            {
                return BadRequest($"Default value of bonus salary is {_basicBonus} but found {newEmployee.BonusSalary}");
            }

            bool isEmpAdded = await _employeeApiService.AddNewEmployeeAsync(newEmployee, certificatesExist);

            if (!isEmpAdded)
            {
                return BadRequest("Something went wrong unable to add new Employee");
            }
            return Ok("New Employee has been added");

        }

        [HttpPost]
        [Route("add-type")]
        public async Task<IActionResult> AddNewTypeOfEmployee(string name)
        {
            if (GeneralValidator.isEmptyNameOf(name))
            {
                return BadRequest("Employee type can't be empty");
            }

            IEnumerable<GetEmployeeTypeDTO?> allTypes = await _employeeApiService.GetAllEmployeeTypesAsync();
            if (EmployeeTypeValidator.isTypeExistIn(allTypes, name))
            {
                return BadRequest($"Employee type {name} already exist");
            }

            bool isTypeHasBeenAdded = await _employeeApiService.AddNewEmployeeTypeAsync(name);
            if (!isTypeHasBeenAdded)
            {
                return BadRequest("Unable to add new type");
            }

            return Ok("New employee type has been added");
        }

        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateExistingEmployeeData(int id, PutEmployeeDTO? putEmpData)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Id={id} isn't correct");
            }

            if (putEmpData == null)
            {
                return BadRequest("Employee data can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(putEmpData.FirstName) || GeneralValidator.isEmptyNameOf(putEmpData.LastName))
            {
                return BadRequest("First or last name can't be empty");
            }

            if (!EmployeeValidator.isCorrectPeselOf(putEmpData.PESEL))
            {
                return BadRequest("PESEL isn't correct");
            }

            if (!EmployeeValidator.isCorrectSalaryOf(putEmpData.Salary))
            {
                return BadRequest("Salary can't be less or equal 0");
            }

            if (GeneralValidator.isEmptyNameOf(putEmpData.Address.City))
            {
                return BadRequest("City can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(putEmpData.Address.Street))
            {
                return BadRequest("Street can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(putEmpData.Address.BuildingNumber))
            {
                return BadRequest("Building number can't be empty");
            }

            //checking if employee exist
            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(id);
            if (employeeDatabase == null)
            {
                return NotFound("Employee doesn't exist");
            }

            GetEmployeeDTO employeeDetailsDatabase = await _employeeApiService.GetDetailedEmployeeDataAsync(employeeDatabase);
            MapEmployeeDataService employeeDataMapper = new MapEmployeeDataService(employeeDetailsDatabase, putEmpData);
            Employee employeeUpdatedData = employeeDataMapper.GetEmployeeUpdatedData();

            bool isEmployeeUpdated = await _employeeApiService.UpdateExistingEmployeeByIdAsync(id, employeeUpdatedData);
            if (!isEmployeeUpdated)
            {
                return BadRequest("Something went wrong unable to update employee");
            }
            return Ok("Employee has been updated");
        }

        [HttpPut]
        [Route("certificates/by-employee/id")]
        public async Task<IActionResult> UpdateEmployeeCertificates(int id, IEnumerable<PutCertificateDTO> putEmpCertificates)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Employee id={id} isn't correct");
            }

            if (putEmpCertificates == null || putEmpCertificates.Count() == 0)
            {
                return BadRequest("Certificates can't be empty");
            }

            foreach (PutCertificateDTO cert in putEmpCertificates)
            {
                if (GeneralValidator.isEmptyNameOf(cert.Name))
                {
                    return BadRequest("One or more certificates is invalid");
                }
            }

            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(id);
            if (employeeDatabase == null)
            {
                return NotFound("Employee doesn't exist");
            }

            GetEmployeeDTO employeeDetailsDatabase = await _employeeApiService.GetDetailedEmployeeDataAsync(employeeDatabase);
            if (employeeDetailsDatabase.Certificates != null && employeeDetailsDatabase.Certificates.Count() > 0)
            {
                if (putEmpCertificates.Count() > employeeDetailsDatabase.Certificates.Count())
                {
                    return BadRequest($"Employee doesn't have {putEmpCertificates.Count()} certificates. " +
                        $"Currenthy employee have only {employeeDetailsDatabase.Certificates.Count()} certificates");
                }
            }
            else
            {
                return NotFound("Employee certificates not found");
            }

            MapEmployeeCertificatesService employeeCertificatesMapper = new MapEmployeeCertificatesService(employeeDetailsDatabase, putEmpCertificates);
            List<PutCertificateDTO> updatedCertificatesData = employeeCertificatesMapper.GetUpdatedCertificateNames();
            List<int> updatedCertificatesId = employeeCertificatesMapper.GetUpdatedCertificatesId();

            bool isCertificatesHasBeenUpdated = await _employeeApiService.UpdateExistingEmployeeCertificatesByIdAsync(updatedCertificatesData, updatedCertificatesId);
            if (!isCertificatesHasBeenUpdated)
            {
                return BadRequest("Unable to update certificates name");
            }

            return Ok("Employee certificates has been updated");
        }

        [HttpPut]
        [Route("type")]
        public async Task<IActionResult> UpdateEmployeeType(int empId, int typeId, int restaurantId)
        {
            if (!GeneralValidator.isCorrectId(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidator.isCorrectId(typeId))
            {
                return BadRequest($"Employee type id={typeId} is invalid");
            }

            if (!GeneralValidator.isCorrectId(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            string? typeName = "";
            IEnumerable<GetEmployeeTypeDTO?> allTypes = await _employeeApiService.GetAllEmployeeTypesAsync();
            if (allTypes == null || allTypes.Count() == 0)
            {
                return NotFound("Employee types not found");
            }
            else
            {
                bool typeExist = false;
                foreach (var type in allTypes)
                {
                    if (type != null && type.IdType == typeId)
                    {
                        typeExist = true;
                    }
                }

                if (!typeExist)
                {
                    return NotFound($"Type id={typeId} not found");
                }

                typeName = allTypes.Where(t => t?.IdType == typeId).Select(t => t?.Name).FirstOrDefault();
            }

            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee not found");
            }

            Restaurant? restaurantDatabase = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);
            if (restaurantDatabase == null)
            {
                return NotFound("Restaurant not found");
            }

            IEnumerable<EmployeesInRestaurant?> restaurantWorkers = await _employeeApiService.GetEmployeeInRestaurantDataByRestaurantIdAsync(restaurantId);
            if (restaurantWorkers != null)
            {
                bool empExistInRestaurant = false;
                foreach (var worker in restaurantWorkers)
                {
                    if (worker != null && worker.IdType == typeId && worker.IdEmployee == empId)
                    {
                        return BadRequest($"Employee {employeeDatabase.FirstName} has already type {typeName} in restaurant {restaurantDatabase.Name}");
                    }

                    if (worker != null && worker.IdEmployee == empId)
                    {
                        empExistInRestaurant = true;
                    }
                }

                if (!empExistInRestaurant)
                {
                    return NotFound($"Employee {employeeDatabase.FirstName} not found in restaurant {restaurantDatabase.Name}");
                }

                //Owner type has always id=1 
                if (typeId == _idOwnerType)
                {
                    int ownersCount = restaurantWorkers.Where(t => t?.IdType == 1).ToList().Count();
                    if (ownersCount >= 1)
                    {
                        return BadRequest($"Unable to update type to Owner because employee {employeeDatabase.FirstName} has owner type already");
                    }
                }
            }

            bool isEmployeeTypeChanged = await _employeeApiService.UpdateEmployeeTypeAsync(empId, typeId, restaurantId);
            if (!isEmployeeTypeChanged)
            {
                return BadRequest($"Unalbe to change employee type in restaurant {restaurantDatabase.Name}");
            }

            return Ok("Employee type has been updated");
        }
    }
}
