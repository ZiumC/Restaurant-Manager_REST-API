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
using Restaurants_REST_API.DTOs.GetDTO;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeApiService _employeeApiService;
        private readonly IRestaurantApiService _restaurantsApiService;
        private readonly IConfiguration _config;
        private readonly string _ownerTypeName;
        private readonly string _supervisorTypeName;

        public EmployeesController(IEmployeeApiService employeeApiService, IRestaurantApiService restaurantsApiService, IConfiguration config)
        {
            _employeeApiService = employeeApiService;
            _restaurantsApiService = restaurantsApiService;
            _config = config;

            _ownerTypeName = _config["ApplicationSettings:AdministrativeRoles:Owner"];
            _supervisorTypeName = _config["ApplicationSettings:AdministrativeRoles:Supervisor"];

            try
            {
                if (string.IsNullOrEmpty(_ownerTypeName))
                {
                    throw new Exception("Owner type name can't be empty");
                }

                if (string.IsNullOrEmpty(_supervisorTypeName))
                {
                    throw new Exception("Supervisor type name can't be empty");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// Returns all employees details.
        /// </summary>
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

        /// <summary>
        /// Returns employee details.
        /// </summary>
        /// <param name="empId">Employee id</param>
        [HttpGet("{empId}")]
        public async Task<IActionResult> GetEmployeeBy(int empId)
        {
            if (!GeneralValidator.isNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            Employee? employee = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employee == null)
            {
                return NotFound($"Employee id={empId} not found");
            }

            return Ok(await _employeeApiService.GetEmployeeDetailsAsync(employee));
        }

        /// <summary>
        /// Returns all supervisors details from any restaurants.
        /// </summary>
        [HttpGet("supervisors")]
        public async Task<IActionResult> GetSupervisors()
        {
            IEnumerable<GetEmployeeTypeDTO>? types = await _restaurantsApiService.GetEmployeeTypesAsync();
            if (types == null) 
            {
                return NotFound("Employee types not found");
            }

            int supervisorTypeId = types
                .Where(t => t.Name == _supervisorTypeName)
                .Select(t => t.IdType)
                .FirstOrDefault();

            if (supervisorTypeId == 0)
            {
                return NotFound("Supervisor type not found");

            }

            IEnumerable<GetEmployeeDTO>? getAllSupervisors = await _employeeApiService.GetAllEmployeesDetailsByTypeIdAsync(supervisorTypeId);
            if (getAllSupervisors == null || getAllSupervisors.Count() == 0)
            {
                return NotFound("Supervisors not found");
            }

            return Ok(getAllSupervisors);
        }

        /// <summary>
        /// Returns supervisor details from any restaurant.
        /// </summary>
        /// <param name="supervisorId">Supervisor id</param>
        [HttpGet("supervisor/{supervisorId}")]
        public async Task<IActionResult> GetSupervisorBy(int supervisorId)
        {
            if (!GeneralValidator.isNumberGtZero(supervisorId))
            {
                return BadRequest($"Supervisor id={supervisorId} is invalid");
            }

            IEnumerable<GetEmployeeTypeDTO>? types = await _restaurantsApiService.GetEmployeeTypesAsync();
            if (types == null)
            {
                return NotFound("Employee types not found");
            }

            int supervisorTypeId = types
                .Where(t => t.Name == _supervisorTypeName)
                .Select(t => t.IdType)
                .FirstOrDefault();

            if (supervisorTypeId == 0)
            {
                return NotFound("Supervisor type not found");
            }

            IEnumerable<GetEmployeeDTO>? getAllSupervisors = await _employeeApiService.GetAllEmployeesDetailsByTypeIdAsync(supervisorTypeId);
            if (getAllSupervisors == null || getAllSupervisors.Count() == 0)
            {
                return NotFound("Supervisors not found");
            }

            GetEmployeeDTO? supervisorData = getAllSupervisors.Where(s => s.IdEmployee == supervisorId).FirstOrDefault();
            if (supervisorData == null)
            {
                return NotFound("Supervisor not found");
            }
            return Ok(supervisorData);
        }

        /// <summary>
        /// Returns owner details.
        /// </summary>
        [HttpGet("owner")]
        public async Task<IActionResult> GetOwnerDetails()
        {
            IEnumerable<GetEmployeeTypeDTO>? types = await _restaurantsApiService.GetEmployeeTypesAsync();
            if (types == null)
            {
                return NotFound("Employee types not found");
            }

            int ownerTypeId = types
                .Where(t => t.Name == _ownerTypeName)
                .Select(t => t.IdType)
                .FirstOrDefault();

            if (ownerTypeId == 0)
            {
                return NotFound("Owner type not found");
            }

            GetEmployeeDTO? getAllOwners = await _employeeApiService.GetEmployeeDetailsByTypeIdAsync(ownerTypeId);
            if (getAllOwners == null)
            {
                return NotFound("Owner not found");
            }

            return Ok(getAllOwners);
        }

        /// <summary>
        /// Returns employees details based on restaurant id.
        /// </summary>
        /// <param name="restaurantId">Restaurant id</param>
        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetEmployeeByRestaurant(int restaurantId)
        {
            if (!GeneralValidator.isNumberGtZero(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound($"Restaurant id={restaurantId} not found");
            }

            IEnumerable<GetEmployeeDTO> employeesInRestaurant = await _employeeApiService.GetEmployeeDetailsByRestaurantIdAsync(restaurantId);

            if (employeesInRestaurant.Count() == 0)
            {
                return NotFound($"Employees not found");
            }

            return Ok(employeesInRestaurant);
        }

        /// <summary>
        /// Adds new employee.
        /// </summary>
        /// <param name="newEmployee">Employee details data</param>
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

        /// <summary>
        /// Adds new certificate to employee based on employee id.
        /// </summary>
        /// <param name="empId">Employee id</param>
        /// <param name="newCertificates">Certificates data</param>
        [HttpPost("{empId}/certificate")]
        public async Task<IActionResult> AddCertificateBy(int empId, IEnumerable<PostCertificateDTO> newCertificates)
        {
            if (!GeneralValidator.isNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
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

            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee not found");
            }

            bool isCertificateHasBeenAdded = await _employeeApiService.AddNewEmployeeCertificatesAsync(empId, newCertificates);
            if (!isCertificateHasBeenAdded)
            {
                return BadRequest($"Something went wrong unable to add certificates to employee {employeeDatabase.FirstName}");
            }

            return Ok($"Certificate has been added to employee {employeeDatabase.FirstName}");
        }

        /// <summary>
        /// Updates existing employee basic data.
        /// </summary>
        /// <param name="empId">Employee id</param>
        /// <param name="putEmpData">Basic employee data</param>
        [HttpPut("{empId}")]
        public async Task<IActionResult> UpdateEmployeeDataBy(int empId, PutEmployeeDTO? putEmpData)
        {
            if (!GeneralValidator.isNumberGtZero(empId))
            {
                return BadRequest($"Id={empId} isn't correct");
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

            decimal minimumBonus = decimal.Parse(_config["ApplicationSettings:BasicBonus"]);
            if (!GeneralValidator.isCorrectBonus(putEmpData.BonusSalary, minimumBonus))
            {
                return BadRequest("Bonus salary is invalid");
            }
            //checking if employee exist
            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee doesn't exist");
            }

            GetEmployeeDTO employeeDetailsDatabase = await _employeeApiService.GetEmployeeDetailsAsync(employeeDatabase);
            MapEmployeeDataService employeeDataMapper = new MapEmployeeDataService(employeeDetailsDatabase, putEmpData);
            Employee employeeUpdatedData = employeeDataMapper.GetEmployeeUpdatedData();

            bool isEmployeeUpdated = await _employeeApiService.UpdateEmployeeDataByIdAsync(empId, employeeUpdatedData);
            if (!isEmployeeUpdated)
            {
                return BadRequest("Something went wrong unable to update employee");
            }
            return Ok("Employee has been updated");
        }

        /// <summary>
        /// Updates existing employee certificate.
        /// </summary>
        /// <param name="empId">Employee id</param>
        /// <param name="certificateId">Certificate id to update</param>
        /// <param name="putEmpCertificates">Certificate data to update</param>
        [HttpPut("{empId}/certificate/{certificateId}")]
        public async Task<IActionResult> UpdateEmployeeCertificatesBy(int empId, int certificateId, PutCertificateDTO putEmpCertificates)
        {
            if (!GeneralValidator.isNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidator.isNumberGtZero(certificateId))
            {
                return BadRequest($"Certificate id={certificateId} is invalid");
            }

            if (GeneralValidator.isEmptyNameOf(putEmpCertificates.Name))
            {
                return BadRequest("Certificate has empty name");
            }

            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee doesn't exist");
            }

            GetEmployeeDTO employeeDetailsDatabase = await _employeeApiService.GetEmployeeDetailsAsync(employeeDatabase);
            if (employeeDetailsDatabase.Certificates != null && employeeDetailsDatabase.Certificates.Count() > 0)
            {
                GetCertificateDTO? employeeCertificate = employeeDetailsDatabase.Certificates
                    .Where(ec => ec.IdCertificate == certificateId)
                    .FirstOrDefault();

                if (employeeCertificate == null)
                {
                    return NotFound($"Employee certificate id={certificateId} not found");
                }
            }
            else
            {
                return NotFound("Employee certificates not found");
            }

            MapEmployeeCertificatesService employeeCertificateMapper = new MapEmployeeCertificatesService(employeeDetailsDatabase, putEmpCertificates, certificateId);
            PutCertificateDTO updatedCertificateData = employeeCertificateMapper.GetUpdatedCertificateNames();

            bool isCertificatesHasBeenUpdated = await _employeeApiService.UpdateEmployeeCertificatesByIdAsync(certificateId, updatedCertificateData);
            if (!isCertificatesHasBeenUpdated)
            {
                return Problem("Unable to update certificate");
            }

            return Ok("Employee certificate has been updated");
        }

        /// <summary>
        /// Removes employee data.
        /// </summary>
        /// <param name="empId">Employee id</param>
        [HttpDelete("{empId}")]
        public async Task<IActionResult> DeleteEmployeeBy(int empId)
        {
            if (!GeneralValidator.isNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound($"Employee not found");
            }

            GetEmployeeDTO employeeDetailsDatabase = await _employeeApiService.GetEmployeeDetailsAsync(employeeDatabase);
            bool isEmployeeHasBeenRemoved = await _employeeApiService.DeleteEmployeeDataByIdAsync(empId, employeeDetailsDatabase);
            if (!isEmployeeHasBeenRemoved)
            {
                return BadRequest("Something went wrong unable to delete employee");
            }

            return Ok($"Employee has been removed");
        }

        /// <summary>
        /// Removes employee certificate data.
        /// </summary>
        /// <param name="empId">Employee id</param>
        /// <param name="certificateId">Employee certificate id</param>
        [HttpDelete("{empId}/certificate/{certificateId}")]
        public async Task<IActionResult> DeleteEmployeeCertificateBy(int empId, int certificateId)
        {
            if (!GeneralValidator.isNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidator.isNumberGtZero(certificateId))
            {
                return BadRequest($"Certificate id={certificateId} is invalid");
            }

            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee not found");
            }

            GetEmployeeDTO employeeDetailsDatabase = await _employeeApiService.GetEmployeeDetailsAsync(employeeDatabase);
            if (employeeDetailsDatabase.Certificates == null || employeeDetailsDatabase.Certificates.Count() == 0)
            {
                return NotFound("Employee certificates not found");
            }

            GetCertificateDTO? empCertificate = employeeDetailsDatabase.Certificates.Where(ec => ec.IdCertificate == certificateId).FirstOrDefault();
            if (empCertificate == null)
            {
                return NotFound($"Certificate id={certificateId} not found in employee {employeeDetailsDatabase.FirstName}");
            }

            bool isCertificateHasBeenDeleted = await _employeeApiService.DeleteEmployeeCertificateAsync(empId, empCertificate);
            if (!isCertificateHasBeenDeleted)
            {
                return BadRequest("Something went wrong unable to delete certificate");
            }

            return Ok("Certificate has been deleted");
        }
    }
}
