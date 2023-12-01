using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DAOs;
using Restaurants_REST_API.DTOs.GetDTO;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Utils.UserUtility;
using Restaurants_REST_API.Utils.ValidatorService;
using System.Data;
using System.Text.RegularExpressions;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantApiService _restaurantsApiService;
        private readonly IEmployeeApiService _employeeApiService;
        private readonly IConfiguration _config;
        private readonly string _ownerTypeName;
        private readonly string _supervisorTypeName;
        private readonly string _ownerRegex;
        private readonly string _newComplaintStatus;
        private readonly string _acceptedComplaintStatus;
        private readonly string _pendingComplaintStatus;
        private readonly string _rejectedComplaintStatus;
        private readonly string _ownerRole;
        private readonly string _chefRole;
        private readonly string _chefHelperRole;
        private readonly string _waiterRole;

        public RestaurantsController(IRestaurantApiService restaurantsApiService, IEmployeeApiService employeeApiService, IConfiguration config)
        {
            _restaurantsApiService = restaurantsApiService;
            _employeeApiService = employeeApiService;
            _config = config;

            _ownerTypeName = _config["ApplicationSettings:AdministrativeRoles:Owner"];
            _supervisorTypeName = _config["ApplicationSettings:AdministrativeRoles:Supervisor"];

            _ownerRegex = _config["ApplicationSettings:DataValidation:OwnerRegex"];

            _newComplaintStatus = _config["ApplicationSettings:ComplaintStatus:New"];
            _pendingComplaintStatus = _config["ApplicationSettings:ComplaintStatus:Pending"];
            _acceptedComplaintStatus = _config["ApplicationSettings:ComplaintStatus:Accepted"];
            _rejectedComplaintStatus = _config["ApplicationSettings:ComplaintStatus:Rejected"];

            _ownerRole = _config["ApplicationSettings:EmployeeTypes:Owner"];
            _chefRole = _config["ApplicationSettings:EmployeeTypes:Chef"];
            _chefHelperRole = _config["ApplicationSettings:EmployeeTypes:Chef-helper"];
            _waiterRole = _config["ApplicationSettings:EmployeeTypes:Waiter"];

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

                if (string.IsNullOrEmpty(_ownerRegex))
                {
                    throw new Exception("Regex for owner can't be empty");
                }

                if (string.IsNullOrEmpty(_newComplaintStatus))
                {
                    throw new Exception("Complaint status (NEW) can't be empty");
                }

                if (string.IsNullOrEmpty(_pendingComplaintStatus))
                {
                    throw new Exception("Complaint status (PENDING) can't be empty");
                }

                if (string.IsNullOrEmpty(_acceptedComplaintStatus))
                {
                    throw new Exception("Complaint status (ACCEPTED) can't be empty");
                }

                if (string.IsNullOrEmpty(_rejectedComplaintStatus))
                {
                    throw new Exception("Complaint status (REJECTED) can't be empty");
                }

                if (string.IsNullOrEmpty(_ownerRole))
                {
                    throw new Exception("Owner role can't be empty");
                }

                if (string.IsNullOrEmpty(_chefRole))
                {
                    throw new Exception("Chef role can't be empty");
                }

                if (string.IsNullOrEmpty(_chefHelperRole))
                {
                    throw new Exception("Chef helper role can't be empty");
                }

                if (string.IsNullOrEmpty(_waiterRole))
                {
                    throw new Exception("Waiter role can't be empty");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Returns all restaurants details.
        /// </summary>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpGet]
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> GetAllRestaurants()
        {
            IEnumerable<GetRestaurantDTO>? restaurants = await _restaurantsApiService.GetAllRestaurantsAsync();

            if (restaurants == null || restaurants.Count() == 0)
            {
                return NotFound("Restaurants not found");
            }

            return Ok(restaurants);
        }

        /// <summary>
        /// Returns restaurant details.
        /// </summary>
        /// <param name="restaurantId">Restaurant id</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpGet("{restaurantId}")]
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> GetRestaurantData(int restaurantId)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            GetRestaurantDTO? restaurantDetails = await _restaurantsApiService.GetRestaurantDetailedDataAsync(restaurantId);
            if (restaurantDetails == null)
            {
                return NotFound($"Restaurant not found");
            }

            return Ok(restaurantDetails);
        }

        /// <summary>
        /// Returns statistics from all restaurants.
        /// </summary>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpGet("stats")]
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> GetAllRestaurantsStatistics()
        {
            IEnumerable<GetRestaurantDTO>? restaurantsDetails = await _restaurantsApiService.GetAllRestaurantsAsync();

            if (restaurantsDetails == null || restaurantsDetails.Count() == 0)
            {
                return NotFound("Restaurants not found");
            }


            var result = restaurantsDetails.Select(rd => new
            {
                IdRestaurant = rd.IdRestaurant,
                RestaurantName = rd.Name,
                RGrade =
                        rd.RestaurantReservations?
                        .Where(rr => rr.ReservationGrade != null)
                        .Average(rr => rr.ReservationGrade),
                Reservations = rd.RestaurantReservations?.Count(),
                Complaints = new
                {
                    TotalComplaints = rd.RestaurantReservations?.Where(rc => rc.ReservationComplaint != null).Count(),
                    NewComplaints =
                            rd.RestaurantReservations?
                            .Where(rc => rc.ReservationComplaint?.Status == _newComplaintStatus)
                            .Count(),
                    PendingComplaints =
                            rd.RestaurantReservations?
                            .Where(rc => rc.ReservationComplaint?.Status == _pendingComplaintStatus)
                            .Count(),
                    AcceptedComplaints =
                            rd.RestaurantReservations?
                            .Where(rc => rc.ReservationComplaint?.Status == _acceptedComplaintStatus)
                            .Count(),
                    RejectedComplaints =
                            rd.RestaurantReservations?
                            .Where(rc => rc.ReservationComplaint?.Status == _rejectedComplaintStatus)
                            .Count(),
                },
                Employees = new
                {
                    TotalSalary = _employeeApiService.GetAllEmployeesDetailsByRestaurantIdAsync(rd.IdRestaurant).Result?.Sum(a => a.Salary),
                    TotalBonus = _employeeApiService.GetAllEmployeesDetailsByRestaurantIdAsync(rd.IdRestaurant).Result?.Sum(a => a.BonusSalary),
                    AllEmployees = rd.RestaurantWorkers?.Count(),
                    TypesCount = new 
                    {
                        Owner = rd.RestaurantWorkers?.Where(rw => rw.EmployeeType.Equals(_ownerRole)).Count(),
                        Chef = rd.RestaurantWorkers?.Where(rw => rw.EmployeeType.Equals(_chefRole)).Count(),
                        ChefHelper = rd.RestaurantWorkers?.Where(rw => rw.EmployeeType.Equals(_chefHelperRole)).Count(),
                        Waiter = rd.RestaurantWorkers?.Where(rw => rw.EmployeeType.Equals(_waiterRole)).Count(),
                    }
                }
            });

            return Ok(result);
        }

        /// <summary>
        /// Returns dishes data with restaurant name with dish exists.
        /// </summary>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpGet("dishes")]
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> GetAllDishes()
        {
            IEnumerable<GetDishDTO>? allDishes = await _restaurantsApiService.GetAllDishesWithRestaurantsAsync();
            if (allDishes == null || allDishes.Count() == 0)
            {
                return NotFound("Dishes not found");
            }

            var dataToReturn = allDishes.Select(ad => new
            {
                IdDish = ad.IdDish,
                DishName = ad.Name,
                DishPrice = ad.Price,
                Restaurants = ad.Restaurants?.Select(r => r.Name).ToList()
            });

            return Ok(dataToReturn);
        }
        /// <summary>
        /// Returns dish details.
        /// </summary>
        /// <param name="dishId">Dish id</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpGet("dish/{dishId}")]
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> GetDishData(int dishId)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(dishId))
            {
                return BadRequest($"Dish id={dishId} is invalid");
            }

            Dish? dishDetailsData = await _restaurantsApiService.GetDishSimpleDataByIdAsync(dishId);
            if (dishDetailsData == null)
            {
                return NotFound($"Dish id={dishId} not found");
            }

            return Ok(dishDetailsData);
        }

        /// <summary>
        /// Returns all employee types.
        /// </summary>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpGet("employee/types")]
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> GetAllEmployeeTypes()
        {
            var types = await _restaurantsApiService.GetEmployeeTypesAsync();
            if (types == null)
            {
                return NotFound("Employee types not found");
            }

            return Ok(types);
        }

        /// <summary>
        /// Adds new restaurant.
        /// </summary>
        /// <param name="newRestaurant">New restaurant basic data</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpPost]
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> AddNewRestaurant(PostRestaurantDTO newRestaurant)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Restaurant data is invalid");
            }

            if (string.IsNullOrEmpty(newRestaurant.Name))
            {
                return BadRequest("Restaurant name can't be empty");
            }

            if (string.IsNullOrEmpty(newRestaurant.Address.City))
            {
                return BadRequest("City adress can't be empty");
            }

            if (string.IsNullOrEmpty(newRestaurant.Address.Street))
            {
                return BadRequest("Street adress can't be empty");
            }

            if (string.IsNullOrEmpty(newRestaurant.Address.BuildingNumber))
            {
                return BadRequest("Building number adress can't be empty");
            }

            if (string.IsNullOrEmpty(newRestaurant.Address.LocalNumber))
            {
                newRestaurant.Address.LocalNumber = null;
            }

            if (string.IsNullOrEmpty(newRestaurant.Status))
            {
                return BadRequest("Restaurant status can't be empty");
            }

            IEnumerable<GetRestaurantDTO>? allRestaurants = await _restaurantsApiService.GetAllRestaurantsAsync();
            if (allRestaurants != null && allRestaurants.Count() > 0)
            {
                GetRestaurantDTO? restaurant = allRestaurants
                    .Where(ar => ar.Name.ToLower() == newRestaurant.Name.ToLower())
                    .FirstOrDefault();

                if (restaurant != null)
                {
                    return BadRequest("Restaurant already exist");
                }
            }

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
                return NotFound("Owner type not found, unable to add new restaurant");
            }

            GetEmployeeDTO? ownerData = await _employeeApiService.GetEmployeeDetailsByTypeIdAsync(ownerTypeId);
            if (ownerData == null)
            {
                return NotFound("Owner of restaurants not found, unable to add new restaurant");
            }

            var restaurantDao = new RestaurantDAO
            {
                Name = newRestaurant.Name,
                Status = newRestaurant.Status,
                BonusBudget = newRestaurant.BonusBudget,
                Address = new AddressDAO
                {
                    City = newRestaurant.Address.City,
                    Street = newRestaurant.Address.Street,
                    BuildingNumber = newRestaurant.Address.BuildingNumber,
                    LocalNumber = newRestaurant.Address.LocalNumber,
                }
            };

            bool isRestaurantAdded = await _restaurantsApiService.AddNewRestaurantAsync(restaurantDao, ownerTypeId);
            if (!isRestaurantAdded)
            {
                return BadRequest("Something went wrong unable to add new restaruant");
            }
            return Ok("Restaurant has been added");
        }

        /// <summary>
        /// Adds new dish.
        /// </summary>
        /// <param name="newDish">New dish basic data</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpPost("dish")]
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> AddNewDishToRestaurant(PostDishDTO newDish)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dish data is invalid");
            }

            if (newDish.IdRestaurants == null || newDish.IdRestaurants.Count() == 0)
            {
                return BadRequest("Dish should be assigned at least to one restaurant");
            }

            if (string.IsNullOrEmpty(newDish.Name))
            {
                return BadRequest("Dish name can't be empty");
            }

            //checking if restaurant exist and does dish exist already in restaurant
            foreach (int restaurantId in newDish.IdRestaurants)
            {
                if (!GeneralValidatorUtility.isIntNumberGtZero(restaurantId))
                {
                    return BadRequest($"Restaurant id={restaurantId} isn't correct");
                }

                GetRestaurantDTO? restaurantDetails = await _restaurantsApiService.GetRestaurantDetailedDataAsync(restaurantId);
                if (restaurantDetails == null)
                {
                    return NotFound($"Restaurant at id={restaurantId} not found");
                }

                IEnumerable<Dish>? dishes = await _restaurantsApiService.GetAllDishesAsync();
                if (dishes != null)
                {
                    foreach (Dish dish in dishes)
                    {
                        if (dish.Name.ToLower().Replace("\\s", "").Equals(newDish.Name.ToLower().Replace("\\s", "")))
                        {
                            return BadRequest($"Dish {newDish.Name} already exist");
                        }
                    }
                }

                if (restaurantDetails.RestaurantDishes != null && restaurantDetails.RestaurantDishes.Count() > 0)
                {
                    foreach (Dish dish in restaurantDetails.RestaurantDishes)
                    {
                        if (dish.Name.Equals(newDish.Name))
                        {
                            return BadRequest($"Dish {dish.Name} already exist in restaurant {restaurantDetails.Name}");
                        }
                    }
                }
            }

            var dishDao = new DishDAO
            {
                Name = newDish.Name,
                Price = newDish.Price
            };

            bool isDishAdded = await _restaurantsApiService.AddNewDishToRestaurantsAsync(dishDao, newDish.IdRestaurants);
            if (!isDishAdded)
            {
                return BadRequest("Something went wrong unable to add new dish");
            }

            return Ok("Dish has been added");
        }

        /// <summary>
        /// Adds existing dish to restaurant.
        /// </summary>
        /// <param name="dishId">Dish id</param>
        /// <param name="restaurantId">Restaurant id</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpPost("{restaurantId}/dish/{dishId}")]
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> AddExistingDishToRestaurant(int restaurantId, int dishId)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(dishId))
            {
                return BadRequest($"Dish id={dishId} is invalid");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            Restaurant? simpleRestaurantData =
                await _restaurantsApiService.GetRestaurantSimpleDataByIdAsync(restaurantId);
            if (simpleRestaurantData == null)
            {
                return NotFound("Restaurant not found");
            }

            Dish? simpleDishData =
                await _restaurantsApiService.GetDishSimpleDataByIdAsync(dishId);
            if (simpleDishData == null)
            {
                return NotFound("Dish not found");
            }

            IEnumerable<GetDishDTO>? allDishes = await _restaurantsApiService.GetAllDishesWithRestaurantsAsync();
            if (allDishes != null)
            {
                GetDishDTO? dish = allDishes
                    .Where
                    (ad =>
                        ad.IdDish == dishId &&
                        ad.Restaurants != null &&
                        ad.Restaurants.Any(a => a.IdRestaurant == restaurantId)
                    )
                    .FirstOrDefault();

                if (dish != null)
                {
                    return BadRequest($"Dish {simpleDishData.Name} already exist in restaurant {simpleRestaurantData.Name}");
                }
            }

            bool isDishAdded = await _restaurantsApiService.AddExistingDishToRestaurantAsync(dishId, restaurantId);
            if (!isDishAdded)
            {
                return BadRequest("Something went wrong, unable to add dish to restaurant");
            }

            return Ok($"Dish {simpleDishData.Name} has been added to restaurant {simpleRestaurantData.Name}");

        }

        /// <summary>
        /// Adds existing employee to restaurant with specified role. 
        /// Employee only can have a one role at a restaurant.
        /// </summary>
        /// <param name="empId">Employee id</param>
        /// <param name="typeId">Employee type id</param>
        /// <param name="restaurantId">Restaurant id</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        /*
         * Only one owner can be hired at once in restaurant
         * Chef can be hired multiple times in restaurant
         */
        [HttpPost("{restaurantId}/employee/{empId}/type/{typeId}")]
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> AddNewEmployeeToRestaurant(int restaurantId, int empId, int typeId)
        {

            if (!GeneralValidatorUtility.isIntNumberGtZero(empId))
            {
                return BadRequest("Employee id isn't correct");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(typeId))
            {
                return BadRequest("Restaurant id isn't correct");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(restaurantId))
            {
                return BadRequest("Restaurant id isn't correct");
            }

            //checking if types exist in db
            IEnumerable<GetEmployeeTypeDTO>? allTypes = await _restaurantsApiService.GetEmployeeTypesAsync();
            if (allTypes == null || allTypes.Count() == 0)
            {
                return NotFound("Employee types not found");
            }

            int idType = allTypes
                .Where(at => at.IdType == typeId)
                .Select(at => at.IdType)
                .FirstOrDefault();
            if (idType == 0)
            {
                return NotFound($"Type id={typeId} not found");
            }

            //checking if employee exist
            Employee? employeeDatabase = await _employeeApiService.GetEmployeeSimpleDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound($"Employee id={empId} not found");
            }

            //checking if restaurant exist
            Restaurant? restaurantDatabase = await _restaurantsApiService.GetRestaurantSimpleDataByIdAsync(restaurantId);
            if (restaurantDatabase == null)
            {
                return NotFound($"Restaurant id={restaurantId} not found");
            }

            IEnumerable<EmployeeRestaurant>? restaurantWorkers = await _restaurantsApiService.GetHiredEmployeesInRestaurantsAsync();
            if (restaurantWorkers != null && restaurantWorkers.Count() > 0)
            {
                //checking if employee exist in passed restaurant id
                EmployeeRestaurant? employeeHired = restaurantWorkers
                    .Where(rw => rw.IdEmployee == empId && rw.IdRestaurant == restaurantId)
                    .FirstOrDefault();
                if (employeeHired != null)
                {
                    return BadRequest($"Employee {employeeDatabase.FirstName} already works in restaurant {restaurantDatabase.Name}");
                }

                int ownerTypeId = allTypes
                    .Where(t => t.Name == _ownerTypeName)
                    .Select(t => t.IdType)
                    .FirstOrDefault();
                if (ownerTypeId == 0)
                {
                    return NotFound("Owner type not found");

                }

                //checking if owner already exist in selected restaurant
                if (ownerTypeId == typeId)
                {
                    EmployeeRestaurant? owner = restaurantWorkers
                        .Where(t => t?.IdType == ownerTypeId && t.IdRestaurant == restaurantId)
                        .FirstOrDefault();
                    if (owner != null)
                    {
                        return BadRequest($"Employee {employeeDatabase.FirstName} can't be hired as an owner because" +
                            $" owner already exists in restaurant {restaurantDatabase.Name}");
                    }

                    if (!Regex.Match(employeeDatabase.IsOwner, _ownerRegex, RegexOptions.IgnoreCase).Success)
                    {
                        return BadRequest($"Employee {employeeDatabase.FirstName} isn't an owner");
                    }
                }
            }

            bool isSupervisorInRestaurant = allTypes
                .Where(at => at.Name == _ownerTypeName || at.Name == _supervisorTypeName)
                .Select(at => at.IdType)
                .ToList()
                .Contains(typeId);

            bool isEmployeeHired = await _restaurantsApiService.AddNewEmployeeToRestaurantAsync(empId, typeId, restaurantId, isSupervisorInRestaurant);
            if (!isEmployeeHired)
            {
                return BadRequest("Something went wrong unable to hire employee");
            }

            return Ok($"Employee {employeeDatabase.FirstName} has been hired in restaurant {restaurantDatabase.Name}");
        }

        /// <summary>
        /// Updates employee type in restaurant.
        /// </summary>
        /// <param name="empId">Employee id</param>
        /// <param name="typeId">Employee type id</param>
        /// <param name="restaurantId">Restaurant id</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpPut("{restaurantId}/employee/{empId}/type/{typeId}")]
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> UpdateEmployeeRoleInRestaurant(int restaurantId, int empId, int typeId)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(typeId))
            {
                return BadRequest($"Employee type id={typeId} is invalid");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            //checking if types exist in db
            IEnumerable<GetEmployeeTypeDTO>? allTypes = await _restaurantsApiService.GetEmployeeTypesAsync();
            if (allTypes == null || allTypes.Count() == 0)
            {
                return NotFound("Employee types not found");
            }

            int idType = allTypes
                .Where(at => at.IdType == typeId)
                .Select(at => at.IdType)
                .FirstOrDefault();
            if (idType == 0)
            {
                return NotFound($"Type id={typeId} not found");
            }

            //checking if employee exist
            Employee? employeeDatabase = await _employeeApiService.GetEmployeeSimpleDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee not found");
            }

            //checking if restaurant exist
            Restaurant? restaurantDatabase = await _restaurantsApiService.GetRestaurantSimpleDataByIdAsync(restaurantId);
            if (restaurantDatabase == null)
            {
                return NotFound("Restaurant not found");
            }

            IEnumerable<EmployeeRestaurant>? restaurantWorkers = await _restaurantsApiService.GetHiredEmployeesInRestaurantsAsync();
            if (restaurantWorkers != null)
            {
                //checking if employee exist in restaurant
                EmployeeRestaurant? employeeHired = restaurantWorkers
                    .Where(rw => rw.IdEmployee == empId && rw.IdRestaurant == restaurantId)
                    .FirstOrDefault();
                if (employeeHired == null)
                {
                    return NotFound($"Employee {employeeDatabase.FirstName} not found in restaurant {restaurantDatabase.Name}");
                }

                //checking if type are same
                if (employeeHired.IdType == typeId)
                {
                    string typeNameQuery = allTypes
                           .Where(at => at.IdType == typeId)
                           .Select(at => at.Name)
                           .First();

                    return BadRequest($"Employee {employeeDatabase.FirstName} is already hired as {typeNameQuery} " +
                        $"in restaurant {restaurantDatabase.Name}");
                }

                int ownerTypeId = allTypes
                    .Where(at => at.Name == _ownerTypeName)
                    .Select(at => at.IdType)
                    .FirstOrDefault();
                if (ownerTypeId == 0)
                {
                    return NotFound("Owner type not found");
                }

                if (ownerTypeId == typeId)
                {
                    EmployeeRestaurant? owner = restaurantWorkers
                        .Where(t => t?.IdType == ownerTypeId && t.IdRestaurant == restaurantId)
                        .FirstOrDefault();
                    if (owner != null)
                    {
                        return BadRequest($"Employee {employeeDatabase.FirstName} can't be hired as an owner because" +
                            $" owner already exists in restaurant {restaurantDatabase.Name}");
                    }

                    if (!Regex.Match(employeeDatabase.IsOwner, _ownerRegex, RegexOptions.IgnoreCase).Success)
                    {
                        return BadRequest($"Employee {employeeDatabase.FirstName} isn't an owner");
                    }
                }
            }

            bool isSupervisorInRestaurant = allTypes
                .Where(at => at.Name == _ownerTypeName || at.Name == _supervisorTypeName)
                .Select(at => at.IdType)
                .ToList()
                .Contains(typeId);

            bool isEmployeeTypeChanged = await _restaurantsApiService.UpdateEmployeeTypeAsync(empId, typeId, restaurantId, isSupervisorInRestaurant);
            if (!isEmployeeTypeChanged)
            {
                return BadRequest($"Unalbe to update employee role in restaurant {restaurantDatabase.Name}");
            }

            return Ok("Employee role has been updated");
        }

        /// <summary>
        /// Updates restaurant data.
        /// </summary>
        /// <param name="restaurantId">Restaurant id</param>
        /// <param name="putRestaurantData">Restaurant basic data</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpPut("{restaurantId}")]
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> UpdateRestaurantData(int restaurantId, PutRestaurantDTO putRestaurantData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Restaurant data is invalid");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            if (string.IsNullOrEmpty(putRestaurantData.Name))
            {
                return BadRequest("Restaurant name can't be empty");
            }

            if (string.IsNullOrEmpty(putRestaurantData.Status))
            {
                return BadRequest("Restaurant status can't be empty");
            }

            if (string.IsNullOrEmpty(putRestaurantData.Address.City))
            {
                return BadRequest("City can't be empty");
            }

            if (string.IsNullOrEmpty(putRestaurantData.Address.Street))
            {
                return BadRequest("Street can't be empty");
            }

            if (string.IsNullOrEmpty(putRestaurantData.Address.BuildingNumber))
            {
                return BadRequest("Building number can't be empty");
            }

            if (string.IsNullOrEmpty(putRestaurantData.Address.LocalNumber))
            {
                putRestaurantData.Address.LocalNumber = null;
            }

            if (putRestaurantData.BonusBudget != null && putRestaurantData.BonusBudget < 0)
            {
                return BadRequest("Restaurant bonus budget can't be less than 0");
            }

            Restaurant? restaurantDatabase = await _restaurantsApiService.GetRestaurantSimpleDataByIdAsync(restaurantId);
            if (restaurantDatabase == null)
            {
                return NotFound($"Restaurant id={restaurantId} not found");
            }

            var restaurantDao = new RestaurantDAO
            {
                Name = putRestaurantData.Name,
                Status = putRestaurantData.Status,
                BonusBudget = putRestaurantData.BonusBudget,
                Address = new AddressDAO
                {
                    City = putRestaurantData.Address.City,
                    Street = putRestaurantData.Address.Street,
                    BuildingNumber = putRestaurantData.Address.BuildingNumber,
                    LocalNumber = putRestaurantData.Address.LocalNumber
                }
            };

            bool isRestaurantUpdated = await _restaurantsApiService.UpdateRestaurantDataAsync(restaurantId, restaurantDao);
            if (!isRestaurantUpdated)
            {
                return BadRequest("Something went wrong unable to update restaurant data");
            }

            return Ok("Restaurant data has been updated");
        }

        /// <summary>
        /// Updates dish data.
        /// </summary>
        /// <param name="dishId">Dish id</param>
        /// <param name="putDishData">Dish basic data</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpPut("dish/{dishId}")]
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> UpdateDishData(int dishId, PutDishDTO putDishData)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dish data is invalid");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(dishId))
            {
                return BadRequest($"Dish id={dishId} is invalid");
            }

            if (string.IsNullOrEmpty(putDishData.Name))
            {
                return BadRequest("Dish name can't be empty");
            }

            if (putDishData.Price <= new decimal(0.0))
            {
                return BadRequest("Dish price is invalid");
            }

            Dish? dishDetailsDatabase = await _restaurantsApiService.GetDishSimpleDataByIdAsync(dishId);
            if (dishDetailsDatabase == null)
            {
                return NotFound("Dish to update not found");
            }

            var dishDao = new DishDAO
            {
                Name = putDishData.Name,
                Price = putDishData.Price
            };
            bool isDishUpdated = await _restaurantsApiService.UpdateDishDataAsync(dishId, dishDao);
            if (!isDishUpdated)
            {
                return BadRequest("Something went wrong unable to update dish data");
            }

            return Ok("Dish data has been updated");
        }

        /// <summary>
        /// Removes dish data from all restaurants.
        /// </summary>
        /// <param name="dishId">Dish id</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpDelete("dish/{dishId}")]
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> DeleteDishFromEverywhere(int dishId)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(dishId))
            {
                return BadRequest($"Dish id={dishId} is invalid");
            }

            Dish? dishDatabase = await _restaurantsApiService.GetDishSimpleDataByIdAsync(dishId);
            if (dishDatabase == null)
            {
                return NotFound("Dish not found");
            }

            bool isDishHasBeenRemoved = await _restaurantsApiService.DeleteDishAsync(dishId);
            if (!isDishHasBeenRemoved)
            {
                return BadRequest("Something went wrong unable to delete dish");
            }

            return Ok($"Dish {dishDatabase.Name} has been deleted from all restaurants");
        }

        /// <summary>
        /// Removes dish only from specific restaurant.
        /// </summary>
        /// <param name="restaurantId">Restaurant id</param>
        /// <param name="dishId">Dish id</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// - Supervisor.
        /// </remarks>
        [HttpDelete("{restaurantId}/dish/{dishId}")]
        [Authorize(Roles = UserRolesUtility.OwnerAndSupervisor)]
        public async Task<IActionResult> DeleteDishFromRestaurant(int restaurantId, int dishId)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(dishId))
            {
                return BadRequest($"Dish id={dishId} is invalid");
            }

            Dish? dishDatabase = await _restaurantsApiService.GetDishSimpleDataByIdAsync(dishId);
            if (dishDatabase == null)
            {
                return NotFound("Dish not found");
            }

            Restaurant? restaurantDatabase = await _restaurantsApiService.GetRestaurantSimpleDataByIdAsync(restaurantId);
            if (restaurantDatabase == null)
            {
                return NotFound("Restaurant not found");
            }

            IEnumerable<RestaurantDish>? restaurantDishes = await _restaurantsApiService.GetRestaurantDishesByRestaurantIdAsync(restaurantId);
            if (restaurantDishes == null || restaurantDishes.Count() == 0)
            {
                return NotFound($"Dishes in restaurant {restaurantDatabase.Name} not found");
            }

            RestaurantDish? restaurantDish = restaurantDishes.Where(rd => rd?.IdDish == dishId).FirstOrDefault();
            if (restaurantDish == null)
            {
                return NotFound("Dish not found in restaurant");
            }

            bool isDishHasBeenRenoved = await _restaurantsApiService.DeleteDishFromRestaurantAsync(restaurantId, dishId);
            if (!isDishHasBeenRenoved)
            {
                return BadRequest("Something went wrong unable to remove dish");
            }

            return Ok($"Dish {dishDatabase.Name} has been removed from restaurant {restaurantDatabase.Name}");
        }

        /// <summary>
        /// Removes employee only from specific restaurant.
        /// </summary>
        /// <param name="empId">Employee id</param>
        /// <param name="restaurantId">Restaurant id</param>
        /// <remarks>
        /// To use that endpoint, access token should contain following roles:
        /// - Owner.
        /// </remarks>
        [HttpDelete("{restaurantId}/employee/{empId}")]
        [Authorize(Roles = UserRolesUtility.Owner)]
        public async Task<IActionResult> DeleteEmployeeFromRestaurant(int empId, int restaurantId)
        {
            if (!GeneralValidatorUtility.isIntNumberGtZero(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidatorUtility.isIntNumberGtZero(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            Employee? employeeDatabase = await _employeeApiService.GetEmployeeSimpleDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee not found");
            }

            Restaurant? restaurantDatabase = await _restaurantsApiService.GetRestaurantSimpleDataByIdAsync(restaurantId);
            if (restaurantDatabase == null)
            {
                return NotFound("Restaurant not found");
            }

            IEnumerable<EmployeeRestaurant>? restaurantsWorkers = await _restaurantsApiService.GetHiredEmployeesInRestaurantsAsync();
            if (restaurantsWorkers == null || restaurantsWorkers.Count() == 0)
            {
                return NotFound($"Workers in restaurant {restaurantDatabase.Name} not found");
            }

            EmployeeRestaurant? worker = restaurantsWorkers.Where(rw => rw?.IdRestaurant == restaurantId && rw.IdEmployee == empId).FirstOrDefault();
            if (worker == null)
            {
                return NotFound($"Employee {employeeDatabase.FirstName} not found in restaurant {restaurantDatabase.Name}");
            }

            if (Regex.Match(employeeDatabase.IsOwner, _ownerRegex, RegexOptions.IgnoreCase).Success)
            {
                return BadRequest($"Employee {employeeDatabase.FirstName} can't be deleted because is an owner");
            }

            bool isEmployeeHasBeenRemoved = await _restaurantsApiService.DeleteEmployeeFromRestaurantAsync(empId, restaurantId);
            if (!isEmployeeHasBeenRemoved)
            {
                return BadRequest("Something went wrong unable to delete employee from restaurant");
            }

            return Ok($"Employee {employeeDatabase.FirstName} has been removed from restaurant {restaurantDatabase.Name}");
        }
    }
}
