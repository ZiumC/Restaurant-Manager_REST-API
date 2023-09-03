using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Utils.ValidatorService;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.DTOs.GetDTO;
using System.Data;
using Microsoft.AspNetCore.Authorization;
using System.Text.RegularExpressions;
using Restaurants_REST_API.Utils.UserUtility;
using Restaurants_REST_API.DAOs;

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
        private readonly string _ownerStatusYes;
        private readonly string _ownerStatusNo;

        public EmployeesController(IEmployeeApiService employeeApiService, IRestaurantApiService restaurantsApiService, IConfiguration config)
        {
            decimal acceptedMinBonus = 150;

            _employeeApiService = employeeApiService;
            _restaurantsApiService = restaurantsApiService;
            _config = config;

            _ownerTypeName = _config["ApplicationSettings:AdministrativeRoles:Owner"];
            _supervisorTypeName = _config["ApplicationSettings:AdministrativeRoles:Supervisor"];

            _peselRegex = _config["ApplicationSettings:DataValidation:PeselRegex"];

            _ownerStatusYes = _config["ApplicationSettings:AdministrativeRoles:OwnerStatusYes"];
            _ownerStatusNo = _config["ApplicationSettings:AdministrativeRoles:OwnerStatusNo"];

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

                if (string.IsNullOrEmpty(_ownerStatusYes))
                {
                    throw new Exception("Owner status (YES) can't be empty");
                }

                if (string.IsNullOrEmpty(_ownerStatusNo))
                {
                    throw new Exception("Owner status (NO) can't be empty");
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
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
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
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> GetEmployeeDetails(int empId)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(empId))
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
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> GetAllSupervisors()
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
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> GetSupervisorDetails(int supervisorId)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(supervisorId))
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
        [Authorize(Roles = UserRolesUtility.Owner)]
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
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> GetEmployeeDetailsByRestaurant(int restaurantId)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            Restaurant? restaurant = await _restaurantsApiService.GetRestaurantSimpleDataByIdAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound($"Restaurant not found");
            }

            IEnumerable<GetEmployeeDTO>? restaurantWorkers = await _employeeApiService.GetAllEmployeesDetailsByRestaurantIdAsync(restaurantId);
            if (restaurantWorkers == null || restaurantWorkers.Count() == 0)
            {
                return NotFound($"Employees not found");
            }

            return Ok(restaurantWorkers);
        }

        /// <summary>
        /// Adds new employee.
        /// </summary>
        /// <param name="newEmpData">Employee details data</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> AddNewEmployee(PostEmployeeDTO newEmpData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Employee data is invalid");
            }

            if (string.IsNullOrEmpty(newEmpData.FirstName) || string.IsNullOrEmpty(newEmpData.LastName))
            {
                return BadRequest("First or last name can't be empty");
            }

            if (!Regex.Match(newEmpData.PESEL, _peselRegex, RegexOptions.IgnoreCase).Success)
            {
                return BadRequest("PESEL is invalid");
            }

            if (!GeneralValidatorUtility.isDecimalNumberGtZero(newEmpData.Salary))
            {
                return BadRequest("Salary can't be less or equal 0");
            }

            if (string.IsNullOrEmpty(newEmpData.Address.City))
            {
                return BadRequest("City can't be empty");
            }

            if (string.IsNullOrEmpty(newEmpData.Address.Street))
            {
                return BadRequest("Street can't be empty");
            }

            if (string.IsNullOrEmpty(newEmpData.Address.BuildingNumber))
            {
                return BadRequest("Building number can't be empty");
            }

            if (string.IsNullOrEmpty(newEmpData.Address.LocalNumber))
            {
                newEmpData.Address.LocalNumber = null;
            }

            if (newEmpData.Certificates != null && newEmpData.Certificates.Count() > 0)
            {
                int countOfEmptyCertificateName =
                    newEmpData.Certificates
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
                        ae.FirstName.ToLower().Replace("\\s", "").Equals(newEmpData.FirstName.ToLower().Replace("\\s", "")) &&
                        ae.LastName.ToLower().Replace("\\s", "").Equals(newEmpData.LastName.ToLower().Replace("\\s", "")) &&
                        ae.PESEL.Equals(newEmpData.PESEL)
                    )
                    .FirstOrDefault();
                if (empExists != null)
                {
                    return BadRequest("Employee already exist");
                }
            }


            //checking if newEmp has bonus sal less than minimum value
            if (newEmpData.BonusSalary < _basicBonus)
            {
                return BadRequest($"Bonus salary is less than minimum bonus (min bonus is {_basicBonus})");
            }

            string employeeOwnerStatus;
            if (allEmployees == null || allEmployees.Count() == 0)
            {
                employeeOwnerStatus = _ownerStatusYes;
            }
            else
            {
                employeeOwnerStatus = _ownerStatusNo;
            }

            var empDao = new EmployeeDAO
            {
                FirstName = newEmpData.FirstName,
                LastName = newEmpData.LastName,
                PESEL = newEmpData.PESEL,
                Salary = newEmpData.Salary,
                BonusSalary = newEmpData.Salary,
                Address = new AddressDAO
                {
                    City = newEmpData.Address.City,
                    Street = newEmpData.Address.Street,
                    BuildingNumber = newEmpData.Address.BuildingNumber,
                    LocalNumber = newEmpData.Address.LocalNumber
                },
                Certificates = newEmpData.Certificates?.Select(c => new CertificateDAO
                {
                    Name= c.Name,
                    ExpirationDate = c.ExpirationDate
                }).ToList(),
            };

            bool isEmpAdded = await _employeeApiService.AddNewEmployeeAsync(empDao, employeeOwnerStatus);
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
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> AddCertificateToEmployee(int empId, IEnumerable<PostCertificateDTO> newCertificates)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Certificates data are invalid");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(empId))
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

            Employee? employeeDatabase = await _employeeApiService.GetEmployeeSimpleDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee not found");
            }

            var certificatesDao = newCertificates.Select(nc => new CertificateDAO
            {
                Name = nc.Name,
                ExpirationDate = nc.ExpirationDate
            }).ToList();

            bool isCertificateHasBeenAdded = await _employeeApiService.AddNewEmployeeCertificatesAsync(empId, certificatesDao);
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
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> UpdateEmployeeData(int empId, PutEmployeeDTO putEmpData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Employee data is invalid");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(empId))
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

            if (!GeneralValidatorUtility.isDecimalNumberGtZero(putEmpData.Salary))
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
            Employee? employeeDatabase = await _employeeApiService.GetEmployeeSimpleDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee doesn't exist");
            }

            var empDao = new EmployeeDAO
            {
                FirstName = putEmpData.FirstName,
                LastName = putEmpData.LastName,
                PESEL = putEmpData.PESEL,
                Salary = putEmpData.Salary,
                BonusSalary = putEmpData.Salary,
                Address = new AddressDAO
                {
                    City = putEmpData.Address.City,
                    Street = putEmpData.Address.Street,
                    BuildingNumber = putEmpData.Address.BuildingNumber,
                    LocalNumber = putEmpData.Address.LocalNumber
                }
            };

            bool isEmployeeUpdated = await _employeeApiService.UpdateEmployeeDataByIdAsync(empId, empDao);
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
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> UpdateEmployeeCertificates(int empId, int certificateId, PutCertificateDTO putEmpCertificates)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(certificateId))
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

            var certificateDao = new CertificateDAO
            {
                Name = putEmpCertificates.Name,
                ExpirationDate = putEmpCertificates.ExpirationDate
            };

            bool isCertificatesHasBeenUpdated = 
                await _employeeApiService.UpdateEmployeeCertificateByIdAsync(certificateId, certificateDao);
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
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> DeleteEmployeeFromEverywhere(int empId)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            GetEmployeeDTO? employeeDetails = await _employeeApiService.GetEmployeeDetailsByEmpIdAsync(empId);
            if (employeeDetails == null)
            {
                return NotFound($"Employee not found");
            }

            bool isEmployeeHasBeenRemoved =
                await _employeeApiService.DeleteEmployeeDataByIdAsync(empId);
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
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> DeleteEmployeeCertificate(int empId, int certificateId)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(certificateId))
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

            bool isCertificateHasBeenDeleted = await _employeeApiService.DeleteEmployeeCertificateAsync(certificateId);
            if (!isCertificateHasBeenDeleted)
            {
                return BadRequest("Something went wrong unable to delete certificate");
            }

            return Ok("Certificate has been deleted");
        }
    }
}
