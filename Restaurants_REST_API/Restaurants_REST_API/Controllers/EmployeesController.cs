using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.ValidationService;
using Restaurants_REST_API.Services.ValidatorService;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.Services.UpdateDataService;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.DTOs.GetDTO;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using Restaurants_REST_API.Services;
using System.Text.RegularExpressions;

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
        private readonly string _peselRegex;
        private readonly decimal _basicBonus;

        public EmployeesController(IEmployeeApiService employeeApiService, IRestaurantApiService restaurantsApiService, IConfiguration config)
        {
            decimal acceptedMinBonus = 150;

            _employeeApiService = employeeApiService;
            _restaurantsApiService = restaurantsApiService;
            _config = config;

            _ownerTypeName = _config["ApplicationSettings:AdministrativeRoles:Owner"];
            _supervisorTypeName = _config["ApplicationSettings:AdministrativeRoles:Supervisor"];

            _peselRegex = _config["ApplicationSettings:DataValidation:PeselRegex"];

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

                if (string.IsNullOrEmpty(_peselRegex))
                {
                    throw new Exception("PESEL regex can't be empty");
                }

                _basicBonus = decimal.Parse(_config["ApplicationSettings:BasicBonus"]);
                if (_basicBonus < acceptedMinBonus)
                {
                    throw new Exception($"Bonus should be at least {acceptedMinBonus}");
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
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = UserRolesService.OwnerAndSupervisor)]
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
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpGet("{empId}")]
        [Authorize(Roles = UserRolesService.OwnerAndSupervisor)]
        public async Task<IActionResult> GetEmployeeBy(int empId)
        {
            if (!GeneralValidator.isIntNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            GetEmployeeDTO? employee = await _employeeApiService.GetEmployeeDetailsByEmpIdAsync(empId);
            if (employee == null)
            {
                return NotFound($"Employee not found");
            }

            return Ok(employee);
        }

        /// <summary>
        /// Returns all supervisors details from any restaurants.
        /// </summary>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpGet("supervisors")]
        [Authorize(Roles = UserRolesService.Owner)]
        public async Task<IActionResult> GetSupervisors()
        {
            IEnumerable<GetEmployeeTypeDTO>? allTypes = await _restaurantsApiService.GetEmployeeTypesAsync();
            if (allTypes == null)
            {
                return NotFound("Employee types not found");
            }

            int supervisorTypeId = allTypes
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
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpGet("supervisor/{supervisorId}")]
        [Authorize(Roles = UserRolesService.Owner)]
        public async Task<IActionResult> GetSupervisorBy(int supervisorId)
        {
            if (!GeneralValidator.isIntNumberGtZero(supervisorId))
            {
                return BadRequest($"Supervisor id={supervisorId} is invalid");
            }

            IEnumerable<GetEmployeeTypeDTO>? allTypes = await _restaurantsApiService.GetEmployeeTypesAsync();
            if (allTypes == null || allTypes.Count() == 0)
            {
                return NotFound("Employee types not found");
            }

            int supervisorTypeId = allTypes
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

            GetEmployeeDTO? supervisorData = getAllSupervisors
                .Where(s => s.IdEmployee == supervisorId)
                .FirstOrDefault();
            if (supervisorData == null)
            {
                return NotFound("Supervisor not found");
            }

            return Ok(supervisorData);
        }

        /// <summary>
        /// Returns owner details.
        /// </summary>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpGet("owner")]
        [Authorize(Roles = UserRolesService.Owner)]
        public async Task<IActionResult> GetOwnerDetails()
        {
            IEnumerable<GetEmployeeTypeDTO>? allTypes = await _restaurantsApiService.GetEmployeeTypesAsync();
            if (allTypes == null)
            {
                return NotFound("Employee types not found");
            }

            int ownerTypeId = allTypes
                .Where(t => t.Name == _ownerTypeName)
                .Select(t => t.IdType)
                .FirstOrDefault();
            if (ownerTypeId == 0)
            {
                return NotFound("Owner type not found");
            }

            GetEmployeeDTO? getOwner = await _employeeApiService.GetEmployeeDetailsByTypeIdAsync(ownerTypeId);
            if (getOwner == null)
            {
                return NotFound("Owner not found");
            }

            return Ok(getOwner);
        }

        /// <summary>
        /// Returns employees details based on restaurant id.
        /// </summary>
        /// <param name="restaurantId">Restaurant id</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpGet("restaurant/{restaurantId}")]
        [Authorize(Roles = UserRolesService.OwnerAndSupervisor)]
        public async Task<IActionResult> GetEmployeeByRestaurant(int restaurantId)
        {
            if (!GeneralValidator.isIntNumberGtZero(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound($"Restaurant not found");
            }

            IEnumerable<GetEmployeeDTO>? restaurantWorkers = await _employeeApiService.GetEmployeeDetailsByRestaurantIdAsync(restaurantId);
            if (restaurantWorkers == null || restaurantWorkers.Count() == 0)
            {
                return NotFound($"Employees not found");
            }

            return Ok(restaurantWorkers);
        }

        /// <summary>
        /// Adds new employee.
        /// </summary>
        /// <param name="newEmployee">Employee details data</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = UserRolesService.OwnerAndSupervisor)]
        public async Task<IActionResult> AddNewEmployee(PostEmployeeDTO newEmployee)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Employee data is invalid");
            }

            if (string.IsNullOrEmpty(newEmployee.FirstName) || string.IsNullOrEmpty(newEmployee.LastName))
            {
                return BadRequest("First or last name can't be empty");
            }

            if (!Regex.Match(newEmployee.PESEL, _peselRegex, RegexOptions.IgnoreCase).Success)
            {
                return BadRequest("PESEL is invalid");
            }

            if (!GeneralValidator.isDecimalNumberGtZero(newEmployee.Salary))
            {
                return BadRequest("Salary can't be less or equal 0");
            }

            if (string.IsNullOrEmpty(newEmployee.Address.City))
            {
                return BadRequest("City can't be empty");
            }

            if (string.IsNullOrEmpty(newEmployee.Address.Street))
            {
                return BadRequest("Street can't be empty");
            }

            if (string.IsNullOrEmpty(newEmployee.Address.BuildingNumber))
            {
                return BadRequest("Building number can't be empty");
            }

            if (string.IsNullOrEmpty(newEmployee.Address.LocalNumber))
            {
                newEmployee.Address.LocalNumber = null;
            }

            if (newEmployee.Certificates != null && newEmployee.Certificates.Count() > 0)
            {
                int countOfEmptyCertificateName =
                    newEmployee.Certificates
                    .Where(nec => nec.Name.Replace("\\s", "").Equals(""))
                    .ToList()
                    .Count();
                if (countOfEmptyCertificateName > 0)
                {
                    return BadRequest("One or more certificates has empty name");
                }
            }

            //checking if employee exists
            IEnumerable<GetEmployeeDTO>? allEmployees = await _employeeApiService.GetAllEmployeesAsync();
            if (allEmployees != null && allEmployees.Count() > 0)
            {
                GetEmployeeDTO? empExists =
                    allEmployees
                    .Where
                    (ae =>
                        ae.FirstName.ToLower().Replace("\\s", "").Equals(newEmployee.FirstName.ToLower().Replace("\\s", "")) &&
                        ae.LastName.ToLower().Replace("\\s", "").Equals(newEmployee.LastName.ToLower().Replace("\\s", "")) &&
                        ae.PESEL.Equals(newEmployee.PESEL)
                    )
                    .FirstOrDefault();
                if (empExists != null)
                {
                    return BadRequest("Employee already exist");
                }
            }


            //checking if newEmp has bonus sal less than minimum value
            if (newEmployee.BonusSalary < _basicBonus)
            {
                return BadRequest($"Bonus salary is less than minimum bonus (min bonus is {_basicBonus})");
            }

            bool isEmpAdded = await _employeeApiService.AddNewEmployeeAsync(newEmployee);

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
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpPost("{empId}/certificate")]
        [Authorize(Roles = UserRolesService.OwnerAndSupervisor)]
        public async Task<IActionResult> AddCertificateBy(int empId, IEnumerable<PostCertificateDTO> newCertificates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Certificates data are invalid");
            }

            if (!GeneralValidator.isIntNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (newCertificates == null || newCertificates.Count() == 0)
            {
                return BadRequest("Certificates can't be empty");
            }

            IEnumerable<PostCertificateDTO>? certificatesEmptyName =
                newCertificates
                .Where(nc => nc.Name.Replace("\\s", "").Equals(""));
            if (certificatesEmptyName.Count() > 0)
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
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpPut("{empId}")]
        [Authorize(Roles = UserRolesService.OwnerAndSupervisor)]
        public async Task<IActionResult> UpdateEmployeeData(int empId, PutEmployeeDTO putEmpData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Employee data is invalid");
            }

            if (!GeneralValidator.isIntNumberGtZero(empId))
            {
                return BadRequest($"Id={empId} isn't correct");
            }

            if (string.IsNullOrEmpty(putEmpData.FirstName) || string.IsNullOrEmpty(putEmpData.LastName))
            {
                return BadRequest("First or last name can't be empty");
            }

            if (!Regex.Match(putEmpData.PESEL, _peselRegex, RegexOptions.IgnoreCase).Success)
            {
                return BadRequest("PESEL isn't correct");
            }

            if (!GeneralValidator.isDecimalNumberGtZero(putEmpData.Salary))
            {
                return BadRequest("Salary can't be less or equal 0");
            }

            if (string.IsNullOrEmpty(putEmpData.Address.City))
            {
                return BadRequest("City can't be empty");
            }

            if (string.IsNullOrEmpty(putEmpData.Address.Street))
            {
                return BadRequest("Street can't be empty");
            }

            if (string.IsNullOrEmpty(putEmpData.Address.BuildingNumber))
            {
                return BadRequest("Building number can't be empty");
            }

            if (string.IsNullOrEmpty(putEmpData.Address.LocalNumber))
            {
                putEmpData.Address.LocalNumber = null;
            }

            if (putEmpData.BonusSalary < _basicBonus)
            {
                return BadRequest($"Bonus salary is less than minimum bonus (min bonus is {_basicBonus})");
            }

            //checking if employee exist
            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee doesn't exist");
            }

            bool isEmployeeUpdated = await _employeeApiService.UpdateEmployeeDataByIdAsync(empId, putEmpData);
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
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpPut("{empId}/certificate/{certificateId}")]
        [Authorize(Roles = UserRolesService.OwnerAndSupervisor)]
        public async Task<IActionResult> UpdateEmployeeCertificatesBy(int empId, int certificateId, PutCertificateDTO putEmpCertificates)
        {
            if (!GeneralValidator.isIntNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidator.isIntNumberGtZero(certificateId))
            {
                return BadRequest($"Certificate id={certificateId} is invalid");
            }

            if (string.IsNullOrEmpty(putEmpCertificates.Name))
            {
                return BadRequest("Certificate has empty name");
            }

            GetEmployeeDTO? employeeDetails = await _employeeApiService.GetEmployeeDetailsByEmpIdAsync(empId);
            if (employeeDetails == null)
            {
                return NotFound("Employee not found");
            }

            if (employeeDetails.Certificates == null || employeeDetails.Certificates.Count() == 0)
            {
                return NotFound("Employees certificates not found");
            }

            GetCertificateDTO? employeeCertificate = employeeDetails.Certificates
                .Where(ec => ec.IdCertificate == certificateId)
                .FirstOrDefault();
            if (employeeCertificate == null)
            {
                return NotFound($"Employee certificate id={certificateId} not found");
            }

            bool isCertificatesHasBeenUpdated = await _employeeApiService.UpdateEmployeeCertificatesByIdAsync(certificateId, putEmpCertificates);
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
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpDelete("{empId}")]
        [Authorize(Roles = UserRolesService.Owner)]
        public async Task<IActionResult> DeleteEmployeeFromEverywhere(int empId)
        {
            if (!GeneralValidator.isIntNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            GetEmployeeDTO? employeeDetails = await _employeeApiService.GetEmployeeDetailsByEmpIdAsync(empId);
            if (employeeDetails == null)
            {
                return NotFound($"Employee not found");
            }

            bool isEmployeeHasBeenRemoved = await _employeeApiService.DeleteEmployeeDataByIdAsync(empId, employeeDetails);
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
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpDelete("{empId}/certificate/{certificateId}")]
        [Authorize(Roles = UserRolesService.OwnerAndSupervisor)]
        public async Task<IActionResult> DeleteEmployeeCertificate(int empId, int certificateId)
        {
            if (!GeneralValidator.isIntNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidator.isIntNumberGtZero(certificateId))
            {
                return BadRequest($"Certificate id={certificateId} is invalid");
            }

            GetEmployeeDTO? employeeDetails = await _employeeApiService.GetEmployeeDetailsByEmpIdAsync(empId);
            if (employeeDetails == null)
            {
                return NotFound("Employee not found");
            }

            if (employeeDetails.Certificates == null || employeeDetails.Certificates.Count() == 0)
            {
                return NotFound("Employee certificates not found");
            }

            GetCertificateDTO? empCertificate = 
                employeeDetails.Certificates
                .Where(ec => ec.IdCertificate == certificateId)
                .FirstOrDefault();
            if (empCertificate == null)
            {
                return NotFound($"Employee {employeeDetails.FirstName} doen't contains certificate id={certificateId}");
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
