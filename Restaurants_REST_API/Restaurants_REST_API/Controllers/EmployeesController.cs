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

            if (supervisorsId == null || supervisorsId.Count() == 0)
            {
                return NotFound("Supervisors not found");

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
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Id={id} is invalid");
            }

            var supervisorsIdList = await _employeeApiService.GetSupervisorsIdAsync();

            if (supervisorsIdList == null)
            {
                return NotFound($"Supervisors not found");
            }

            if (!supervisorsIdList.Contains(id))
            {
                return NotFound($"Supervisor id={id} not found");
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

            if (GeneralValidator.isEmptyNameOf(newEmployee.Address.LocalNumber))
            {
                newEmployee.Address.LocalNumber = null;
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

        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateEmployeeData(int id, PutEmployeeDTO? putEmpData)
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
