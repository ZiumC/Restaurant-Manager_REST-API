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

namespace Restaurants_REST_API.Controllers
{
    [Route("api/menage/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeApiService _employeeApiService;
        private readonly IRestaurantApiService _restaurantsApiService;
        private readonly decimal _basicBonus = 150;

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

            IEnumerable<EmployeeType?> allTypes = await _employeeApiService.GetAllEmployeeTypesAsync();
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
        [Route("id")]
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
            if (employeeDetailsDatabase.Certificates != null)
            {
                if (putEmpCertificates.Count() > employeeDetailsDatabase.Certificates.Count())
                {
                    return BadRequest($"Employee doesn't have {putEmpCertificates.Count()} certificates. " +
                        $"Currenthy employee have {employeeDetailsDatabase.Certificates.Count()} certificates");
                }
            } else
            {
                return NotFound("Employee certificates not found");
            }


            MapEmployeeCertificatesService employeeCertificatesMapper = new MapEmployeeCertificatesService(employeeDetailsDatabase, putEmpCertificates);
            List<PutCertificateDTO> employeeUpdatedCertificates = employeeCertificatesMapper


            return Ok("employee eertificates has been updated");
        }
    }
}
