﻿using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.ValidationService;
using Restaurants_REST_API.Services.ValidatorService;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.Services.UpdateDataService;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Services.MapperService;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeApiService _employeeApiService;
        private readonly IRestaurantApiService _restaurantsApiService;
        private readonly IConfiguration _config;

        public EmployeesController(IEmployeeApiService employeeApiService, IRestaurantApiService restaurantsApiService, IConfiguration config)
        {
            _employeeApiService = employeeApiService;
            _restaurantsApiService = restaurantsApiService;
            _config = config;
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
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Employee id={id} is invalid");
            }

            Employee? employee = await _employeeApiService.GetBasicEmployeeDataByIdAsync(id);
            if (employee == null)
            {
                return NotFound($"Employee id={id} not found");
            }

            return Ok(await _employeeApiService.GetDetailedEmployeeDataAsync(employee));
        }

        [HttpGet]
        [Route("supervisors")]
        public async Task<IActionResult> GetSupervisors()
        {
            IEnumerable<int>? supervisorsId = await _employeeApiService.GetSupervisorsIdAsync();

            if (supervisorsId == null || supervisorsId.Count() == 0)
            {
                return NotFound("Supervisors not found");

            }

            return Ok(await _employeeApiService.GetDetailedSupervisorsDataAsync((List<int>)supervisorsId));
        }

        /*
         * This method uses GetSupervisorsDetailsAsync(List<int>) because implementation of 
         * GetSupervisorDetailsByIdAsync(int id) in interface IEmployeeApiService would very similar!
         */
        [HttpGet]
        [Route("supervisor/id")]
        public async Task<IActionResult> GetSupervisorBy(int id)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Supervisor id={id} is invalid");
            }

            IEnumerable<int>? supervisorsIdList = await _employeeApiService.GetSupervisorsIdAsync();

            if (supervisorsIdList == null)
            {
                return NotFound($"Supervisors not found");
            }

            if (!supervisorsIdList.Contains(id))
            {
                return NotFound($"Supervisor not found");
            }

            return Ok(await _employeeApiService.GetDetailedSupervisorsDataAsync(new List<int> { id }));
        }

        [HttpGet]
        [Route("owner")]
        public async Task<IActionResult> GetOwnerDetails()
        {
            Employee? ownerDatabase = await _employeeApiService.GetBasicOwnerDataAsync();

            if (ownerDatabase == null)
            {
                return NotFound("Owner not found");
            }

            return Ok(await _employeeApiService.GetDetailedEmployeeDataAsync(ownerDatabase));
        }

        [HttpGet]
        [Route("by-restaurant/id")]
        public async Task<IActionResult> GetEmployeeByRestaurant(int id)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Restaurant id={id} is invalid");
            }

            Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(id);
            if (restaurant == null)
            {
                return NotFound($"Restaurant not found");
            }

            IEnumerable<GetEmployeeDTO> employeesInRestaurant = await _employeeApiService.GetDetailedEmployeeDataByRestaurantIdAsync(id);

            if (employeesInRestaurant.Count() == 0)
            {
                return NotFound($"Employees not found");
            }

            return Ok(employeesInRestaurant);
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

            if (GeneralValidator.isEmptyNameOf(newEmployee.Address.LocalNumber))
            {
                newEmployee.Address.LocalNumber = null;
            }

            bool certificatesExist = false;
            if (newEmployee.Certificates != null && newEmployee.Certificates.Count() > 0)
            {
                certificatesExist = true;

                int countOfEmptyCertificateName = newEmployee.Certificates.Where(nec => nec.Name.Replace("\\s", "").Equals("")).ToList().Count();
                if (countOfEmptyCertificateName > 0)
                {
                    return BadRequest("One or more certificates has empty name");
                }
            }

            //need to check if emp exist in db
            IEnumerable<GetEmployeeDTO> allEmployees = await _employeeApiService.GetAllEmployeesAsync();
            if (EmployeeValidator.isEmployeeExistIn(allEmployees, newEmployee))
            {
                return BadRequest("Employee already exist");
            }

            //parsing basic bonus from app settings
            decimal? basicBonus = null;
            try
            {
                basicBonus = decimal.Parse(_config["ApplicationSettings:BasicBonus"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return BadRequest("Something went wrong, bad value to parse");
            }

            //checking if newEmp has bonus sal less than minimum value
            if (newEmployee.BonusSalary < basicBonus)
            {
                return BadRequest($"Default value of bonus salary is {basicBonus} but found {newEmployee.BonusSalary}");
            }

            bool isEmpAdded = await _employeeApiService.AddNewEmployeeAsync(newEmployee, certificatesExist);

            if (!isEmpAdded)
            {
                return BadRequest("Something went wrong unable to add new Employee");
            }
            return Ok("New Employee has been added");
        }

        [HttpPost]
        [Route("certificate/by-employee/id")]
        public async Task<IActionResult> AddCertificateByEmployee(int id, IEnumerable<PostCertificateDTO> newCertificates)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Employee id={id} is invalid");
            }

            if (newCertificates == null || newCertificates.Count() == 0)
            {
                return BadRequest("Certificates can't be empty");
            }

            int countOfEmptyCertificateNames = newCertificates.Where(nc => nc.Name.Replace("\\s", "").Equals("")).ToList().Count();
            if (countOfEmptyCertificateNames > 0)
            {
                return BadRequest("One or more certificates has empty name");
            }

            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(id);
            if (employeeDatabase == null)
            {
                return NotFound("Employee not found");
            }

            bool isCertificateHasBeenAdded = await _employeeApiService.AddNewEmployeeCertificatesAsync(id, newCertificates);
            if (!isCertificateHasBeenAdded)
            {
                return BadRequest($"Something went wrong unable to add certificates to employee {employeeDatabase.FirstName}");
            }

            return Ok($"Certificate has been added to employee {employeeDatabase.FirstName}");
        }

        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateEmployeeDataByEmployee(int id, PutEmployeeDTO? putEmpData)
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

            bool isEmployeeUpdated = await _employeeApiService.UpdateEmployeeDataByIdAsync(id, employeeUpdatedData);
            if (!isEmployeeUpdated)
            {
                return BadRequest("Something went wrong unable to update employee");
            }
            return Ok("Employee has been updated");
        }

        [HttpPut]
        [Route("certificates/by-employee/id")]
        public async Task<IActionResult> UpdateEmployeeCertificatesByEmployee(int id, IEnumerable<PutCertificateDTO> putEmpCertificates)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Employee id={id} isn't correct");
            }

            if (putEmpCertificates == null || putEmpCertificates.Count() == 0)
            {
                return BadRequest("Certificates can't be empty");
            }


            int countOfEmptyCertificateName = putEmpCertificates.Where(pec => pec.Name.Replace("\\s", "").Equals("")).ToList().Count();
            if (countOfEmptyCertificateName > 0)
            {
                return BadRequest("One or more certificates has empty name");
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

            bool isCertificatesHasBeenUpdated = await _employeeApiService.UpdateEmployeeCertificatesByIdAsync(updatedCertificatesData, updatedCertificatesId);
            if (!isCertificatesHasBeenUpdated)
            {
                return BadRequest("Unable to update certificates name");
            }

            return Ok("Employee certificates has been updated");
        }


        [HttpDelete]
        [Route("id")]
        public async Task<IActionResult> RemoveEmployeeBy(int id)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Employee id={id} is invalid");
            }

            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(id);
            if (employeeDatabase == null)
            {
                return NotFound($"Employee id={id} not found");
            }


            return Ok($"Employee has been removed");
        }
    }
}
