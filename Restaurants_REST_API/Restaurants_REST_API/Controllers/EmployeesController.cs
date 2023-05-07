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
        /// Returns all supervisors details 
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
        /// Returns supervisor details by supervisor id
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
        public async Task<IActionResult> UpdateEmployeeCertificatesByEmployee(int empId, IEnumerable<PutCertificateDTO> putEmpCertificates)
        {
            if (!GeneralValidator.isCorrectId(empId))
            {
                return BadRequest($"Employee id={empId} isn't correct");
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

            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
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

        [HttpDelete]
        [Route("certificate/id")]
        public async Task<IActionResult> DeleteEmployeeCertificateBy(int empId, int certId)
        {
            if (!GeneralValidator.isCorrectId(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidator.isCorrectId(certId))
            {
                return BadRequest($"Certificate id={certId} is invalid");
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

            GetCertificateDTO? empCertificate = employeeDetailsDatabase.Certificates.Where(ec => ec.IdCertificate == certId).FirstOrDefault();
            if (empCertificate == null)
            {
                return NotFound($"Certificate id={certId} not found in employee {employeeDetailsDatabase.FirstName}");
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
