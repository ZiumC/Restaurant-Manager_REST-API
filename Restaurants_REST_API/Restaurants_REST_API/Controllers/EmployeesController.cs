using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        [Route("types")]
        public async Task<IActionResult> GetAllEmployeeTypes()
        {
            var types = await _employeeApiService.GetAllTypesAsync();
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
        [Route("type")]
        public async Task<IActionResult> AddNewTypeOfEmployee(string name)
        {
            if (GeneralValidator.isEmptyNameOf(name))
            {
                return BadRequest("Employee type can't be empty");
            }

            IEnumerable<GetEmployeeTypeDTO?> allTypes = await _employeeApiService.GetAllTypesAsync();
            if (EmployeeTypeValidator.isTypeExistInByName(allTypes, name))
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
            //checking if id are valid
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

            //checking if types exist in db
            IEnumerable<GetEmployeeTypeDTO?> allTypes = await _employeeApiService.GetAllTypesAsync();
            if (!EmployeeTypeValidator.isTypesExist(allTypes))
            {
                return NotFound("Employee types not found in data base");
            }

            //checking if type exist
            if (!EmployeeTypeValidator.isTypeExistInById(allTypes, typeId))
            {
                return NotFound($"Type id={typeId} not found");
            }

            //checking if employee exist
            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee not found");
            }

            //checking if restaurant exist
            Restaurant? restaurantDatabase = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);
            if (restaurantDatabase == null)
            {
                return NotFound("Restaurant not found");
            }

            IEnumerable<EmployeesInRestaurant?> restaurantWorkers = await _restaurantsApiService.GetEmployeeInRestaurantDataByRestaurantIdAsync(restaurantId);
            if (restaurantWorkers != null)
            {
                //checking if employee exist in passed restaurant id
                int? empIdInRestaurantQuery = restaurantWorkers
                    .Where(rw => rw?.IdEmployee == empId)
                    .Select(rw => rw?.IdEmployee)
                    .FirstOrDefault();
                if (empIdInRestaurantQuery == null)
                {
                    return NotFound($"Employee {employeeDatabase.FirstName} not found in restaurant {restaurantDatabase.Name}");
                }

                //checking if employee is already hired as passed type id in passed restaurant id
                int? empIdInRestaurantWithTypeQuery = restaurantWorkers
                    .Where(rw => rw?.IdType == typeId && rw.IdEmployee == empId)
                    .Select(rw => rw?.IdEmployee)
                    .FirstOrDefault();

                if (empIdInRestaurantWithTypeQuery != null)
                {
                    string? typeNameQuery = allTypes
                           .Where(at => at?.IdType == typeId)
                           .Select(at => at?.Name)
                           .FirstOrDefault();

                    return BadRequest($"Employee {employeeDatabase.FirstName} has already type {typeNameQuery} in restaurant {restaurantDatabase.Name}");
                }

                //parsing owner type id from app settings
                int? ownerTypeId = null;
                try
                {
                    ownerTypeId = int.Parse(_config["ApplicationSettings:OwnerTypeId"]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return BadRequest("Something went wrong, bad value to parse");
                }

                //checking if owner already exist 
                if (typeId == ownerTypeId)
                {
                    var ownersCount = restaurantWorkers.Where(t => t?.IdType == 1).ToList();
                    if (ownersCount != null && ownersCount.Count() >= 1)
                    {
                        return BadRequest($"Unable to add type Owner because owner already exists");
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
