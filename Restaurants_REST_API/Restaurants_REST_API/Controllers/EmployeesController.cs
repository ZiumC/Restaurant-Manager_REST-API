using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// Returns all employees details
        /// </summary>
        /// <returns></returns>
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
        /// Returns employee details by employee id
        /// </summary>
        /// <param name="empId">Employee id</param>
        [HttpGet("{empId}")]
        public async Task<IActionResult> GetEmployeeBy(int empId)
        {
            if (!GeneralValidator.isCorrectId(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            Employee? employee = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employee == null)
            {
                return NotFound($"Employee id={empId} not found");
            }

            return Ok(await _employeeApiService.GetDetailedEmployeeDataAsync(employee));
        }

        /// <summary>
        /// Returns all supervisors details from any restaurants
        /// </summary>
        [HttpGet("supervisors")]
        public async Task<IActionResult> GetSupervisors()
        {
            IEnumerable<GetEmployeeDTO>? getAllSupervisors = null;
            try
            {
                getAllSupervisors = await _employeeApiService.GetAllSupervisorsAsync();
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong: {ex.Message}");
            }

            if (getAllSupervisors == null || getAllSupervisors.Count() == 0)
            {
                return NotFound("Supervisors not found");
            }

            return Ok(getAllSupervisors);
        }

        /// <summary>
        /// Returns supervisor details by supervisor id from any restaurant
        /// </summary>
        /// <param name="supervisorId">Supervisor id</param>
        [HttpGet("supervisor/{supervisorId}")]
        public async Task<IActionResult> GetSupervisorBy(int supervisorId)
        {
            if (!GeneralValidator.isCorrectId(supervisorId))
            {
                return BadRequest($"Supervisor id={supervisorId} is invalid");
            }

            Employee? supervisorDatabase = null;
            try
            {
                supervisorDatabase = await _employeeApiService.GetBasicSupervisorDataByIdAsync(supervisorId);
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong: {ex.Message}");
            }

            if (supervisorDatabase == null)
            {
                return NotFound($"Supervisor id={supervisorId} not found");
            }

            GetEmployeeDTO employeeDetailsDatabase = await _employeeApiService.GetDetailedEmployeeDataAsync(supervisorDatabase);

            return Ok(employeeDetailsDatabase);
        }

        /// <summary>
        /// Returns owner details
        /// </summary>
        [HttpGet("owner")]
        public async Task<IActionResult> GetOwnerDetails()
        {
            Employee? ownerDatabase = null;
            try
            {
                ownerDatabase = await _employeeApiService.GetBasicOwnerDataAsync();
            }
            catch (Exception ex)
            {
                return Problem($"Something went wrong: {ex.Message}");
            }

            if (ownerDatabase == null)
            {
                return NotFound("Owner not found");
            }

            GetEmployeeDTO employeeDetailsDatabase = await _employeeApiService.GetDetailedEmployeeDataAsync(ownerDatabase);

            return Ok(employeeDetailsDatabase);
        }

        /// <summary>
        /// Returns employees details by restaurant id
        /// </summary>
        /// <param name="restaurantId">Restaurant id</param>
        [HttpGet("restaurant/{restaurantId}")]
        public async Task<IActionResult> GetEmployeeByRestaurant(int restaurantId)
        {
            if (!GeneralValidator.isCorrectId(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);
            if (restaurant == null)
            {
                return NotFound($"Restaurant id={restaurantId} not found");
            }

            IEnumerable<GetEmployeeDTO> employeesInRestaurant = await _employeeApiService.GetDetailedEmployeeDataByRestaurantIdAsync(restaurantId);

            if (employeesInRestaurant.Count() == 0)
            {
                return NotFound($"Employees not found");
            }

            return Ok(employeesInRestaurant);
        }

        /// <summary>
        /// Adds new employee
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
        /// Adds new certificate to employee by employee id
        /// </summary>
        /// <param name="empId">Employee id</param>
        /// <param name="newCertificates">Certificates data</param>
        [HttpPost("{empId}/certificate")]
        public async Task<IActionResult> AddCertificateByEmployee(int empId, IEnumerable<PostCertificateDTO> newCertificates)
        {
            if (!GeneralValidator.isCorrectId(empId))
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
        /// Updates existing employee basic data by employee id
        /// </summary>
        /// <param name="empId">Employee id</param>
        /// <param name="putEmpData">Basic employee data</param>
        [HttpPut("{empId}")]
        public async Task<IActionResult> UpdateEmployeeDataByEmployee(int empId, PutEmployeeDTO? putEmpData)
        {
            if (!GeneralValidator.isCorrectId(empId))
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

            //checking if employee exist
            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee doesn't exist");
            }

            GetEmployeeDTO employeeDetailsDatabase = await _employeeApiService.GetDetailedEmployeeDataAsync(employeeDatabase);
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
        /// Updates existing employee certificate by employee id and certificate id
        /// </summary>
        /// <param name="empId">Employee id</param>
        /// <param name="certificateId">Certificate id to update</param>
        /// <param name="putEmpCertificates">Certificate data to update</param>
        [HttpPut("{empId}/certificate/{certificateId}")]
        public async Task<IActionResult> UpdateEmployeeCertificatesByEmployee(int empId, int certificateId, PutCertificateDTO putEmpCertificates)
        {
            if (!GeneralValidator.isCorrectId(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidator.isCorrectId(certificateId))
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

            GetEmployeeDTO employeeDetailsDatabase = await _employeeApiService.GetDetailedEmployeeDataAsync(employeeDatabase);
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

            bool isCertificatesHasBeenUpdated = await _employeeApiService.UpdateEmployeeCertificatesByIdAsync(certificateId ,updatedCertificateData);
            if (!isCertificatesHasBeenUpdated)
            {
                return Problem("Unable to update certificate");
            }

            return Ok("Employee certificate has been updated");
        }

        /// <summary>
        /// Removes employee data
        /// </summary>
        /// <param name="empId">Employee id</param>
        [HttpDelete("{empId}")]
        public async Task<IActionResult> DeleteEmployeeBy(int empId)
        {
            if (!GeneralValidator.isCorrectId(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound($"Employee not found");
            }

            GetEmployeeDTO employeeDetailsDatabase = await _employeeApiService.GetDetailedEmployeeDataAsync(employeeDatabase);
            bool isEmployeeHasBeenRemoved = await _employeeApiService.DeleteEmployeeDataByIdAsync(empId, employeeDetailsDatabase);
            if (!isEmployeeHasBeenRemoved)
            {
                return BadRequest("Something went wrong unable to delete employee");
            }

            return Ok($"Employee has been removed");
        }

        /// <summary>
        /// Removes employee certificate data
        /// </summary>
        /// <param name="empId">Employee id</param>
        /// <param name="certificateId">Employee certificate id</param>
        [HttpDelete("{empId}/certificate/{certificateId}")]
        public async Task<IActionResult> DeleteEmployeeCertificateBy(int empId, int certificateId)
        {
            if (!GeneralValidator.isCorrectId(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidator.isCorrectId(certificateId))
            {
                return BadRequest($"Certificate id={certificateId} is invalid");
            }

            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee not found");
            }

            GetEmployeeDTO employeeDetailsDatabase = await _employeeApiService.GetDetailedEmployeeDataAsync(employeeDatabase);
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
