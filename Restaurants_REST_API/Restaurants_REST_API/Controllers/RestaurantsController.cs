using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTO;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.MapperService;
using Restaurants_REST_API.Services.ValidatorService;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/manage/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantApiService _restaurantsApiService;
        private readonly IEmployeeApiService _employeeApiService;
        private readonly IConfiguration _config;

        public RestaurantsController(IRestaurantApiService restaurantsApiService, IEmployeeApiService employeeApiService, IConfiguration config)
        {
            _restaurantsApiService = restaurantsApiService;
            _employeeApiService = employeeApiService;
            _config = config;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRestaurants()
        {
            IEnumerable<GetRestaurantDTO?> restaurants = await _restaurantsApiService.GetAllRestaurantsAsync();

            if (restaurants == null || restaurants.Count() == 0)
            {
                return NotFound("Restaurants not found");
            }

            return Ok(restaurants);
        }
        
        /// <summary>
        /// asdasdasdsdads
        /// </summary>
        /// <param name="restaurantId">asdasdasd</param>
        /// <returns></returns>
        [HttpGet("{restaurantId}")]
        public async Task<IActionResult> GetRestaurantBy(int restaurantId)
        {
            if (!GeneralValidator.isCorrectId(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);

            if (restaurant == null)
            {
                return NotFound($"Restaurant not found");
            }

            GetRestaurantDTO restaurantDTO = await _restaurantsApiService.GetDetailedRestaurantDataAsync(restaurant);

            return Ok(restaurantDTO);
        }
        [HttpGet]
        [Route("dish/{dishId}")]
        public async Task<IActionResult> GetDishBy(int dishId)
        {
            if (!GeneralValidator.isCorrectId(dishId))
            {
                return BadRequest($"Dish id={dishId} is invalid");
            }

            Dish? dishDetailsData = await _restaurantsApiService.GetBasicDishDataByIdAsync(dishId);
            if (dishDetailsData == null)
            {
                return NotFound($"Dish id={dishId} not found");
            }

            return Ok(dishDetailsData);
        }

        [HttpGet]
        [Route("employee/types")]
        public async Task<IActionResult> GetAllEmployeeTypes()
        {
            var types = await _restaurantsApiService.GetEmployeeTypesAsync();
            if (types == null)
            {
                return NotFound("Employee types not found");
            }

            return Ok(types);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewRestaurant(PostRestaurantDTO newRestaurant)
        {
            if (newRestaurant == null)
            {
                return BadRequest("Restaurant must be specified");
            }

            if (GeneralValidator.isEmptyNameOf(newRestaurant.Name))
            {
                return BadRequest("Restaurant name can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(newRestaurant.Address.City))
            {
                return BadRequest("City adress can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(newRestaurant.Address.Street))
            {
                return BadRequest("Street adress can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(newRestaurant.Address.BuildingNumber))
            {
                return BadRequest("Building number adress can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(newRestaurant.Address.LocalNumber))
            {
                newRestaurant.Address.LocalNumber = null;
            }

            if (GeneralValidator.isEmptyNameOf(newRestaurant.Status))
            {
                return BadRequest("Restaurant statuc can't be empty");
            }


            IEnumerable<GetRestaurantDTO?> allRestaurants = await _restaurantsApiService.GetAllRestaurantsAsync();
            if (RestaurantValidator.isRestaurantExistIn(allRestaurants, newRestaurant))
            {
                return BadRequest("Restaurant already exist");
            }

            bool isRestaurantAdded = await _restaurantsApiService.AddNewRestaurantAsync(newRestaurant);
            if (!isRestaurantAdded)
            {
                return BadRequest("Something went wrong unable to add new restaruant");
            }
            return Ok("Restaurant has been added");
        }

        [HttpPost]
        [Route("dish")]
        public async Task<IActionResult> AddNewDish(PostDishDTO newDish)
        {
            if (newDish == null)
            {
                return BadRequest("Dish must be specified");
            }

            if (newDish.IdRestaurants == null)
            {
                return BadRequest("Dish must have specified one or more restaurants");
            }

            if (GeneralValidator.isEmptyNameOf(newDish.Name))
            {
                return BadRequest("Dish name can't be empty");
            }

            if (newDish.IdRestaurants.Count() == 0)
            {
                return BadRequest("Restasurants id are required");
            }

            List<GetRestaurantDTO> restaurants = new List<GetRestaurantDTO>();

            foreach (int restaurantId in newDish.IdRestaurants)
            {
                if (!GeneralValidator.isCorrectId(restaurantId))
                {
                    return BadRequest("One or many restaurant id isn't correct");
                }

                Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);

                if (restaurant == null)
                {
                    return NotFound($"Restaurant id={restaurantId} not found");
                }

                restaurants.Add(await _restaurantsApiService.GetDetailedRestaurantDataAsync(restaurant));
            }

            if (RestaurantValidator.isDishExistIn(restaurants, newDish))
            {
                return BadRequest("Dish already exist in one or many restaurants");
            }

            bool isDishAdded = await _restaurantsApiService.AddNewDishToRestaurantsAsync(newDish);
            if (!isDishAdded)
            {
                return BadRequest("Something went wrong unable to add new dish");
            }

            return Ok("Dish has been added");
        }

        [HttpPost]
        [Route("{restaurantId}/employee/{empId}/type/{typeId}")]
        public async Task<IActionResult> AddNewEmployeeToRestaurant(int empId, int typeId, int restaurantId)
        {

            if (!GeneralValidator.isCorrectId(empId))
            {
                return BadRequest("Employee id isn't correct");
            }

            if (!GeneralValidator.isCorrectId(typeId))
            {
                return BadRequest("Restaurant id isn't correct");
            }

            if (!GeneralValidator.isCorrectId(restaurantId))
            {
                return BadRequest("Restaurant id isn't correct");
            }

            //checking if types exist in db
            IEnumerable<GetEmployeeTypeDTO?> allTypes = await _restaurantsApiService.GetEmployeeTypesAsync();
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
                return NotFound($"Employee id={empId} not found");
            }

            //checking if restaurant exist
            Restaurant? restaurantDatabase = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);
            if (restaurantDatabase == null)
            {
                return NotFound($"Restaurant id={restaurantId} not found");
            }

            IEnumerable<EmployeesInRestaurant?> restaurantWorkers = await _restaurantsApiService.GetHiredEmployeesByRestaurantIdAsync(restaurantId);
            if (restaurantWorkers != null)
            {
                //checking if employee exist in passed restaurant id
                int? empIdInRestaurantQuery = restaurantWorkers
                    .Where(rw => rw?.IdEmployee == empId)
                    .Select(rw => rw?.IdEmployee)
                    .FirstOrDefault();

                if (empIdInRestaurantQuery != null)
                {
                    return BadRequest($"Employee {employeeDatabase.FirstName} already works in restaurant {restaurantDatabase.Name}");
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
                    var ownersCount = restaurantWorkers.Where(t => t?.IdType == ownerTypeId).ToList();
                    if (ownersCount != null && ownersCount.Count() >= 1)
                    {
                        return BadRequest($"Unable to add type Owner because owner already exists");
                    }
                }

            }

            bool isEmployeeHired = await _restaurantsApiService.AddNewEmployeeToRestaurantAsync(empId, typeId, restaurantId);
            if (!isEmployeeHired)
            {
                return BadRequest("Something went wrong unable to hire employee");
            }

            return Ok($"Employee {employeeDatabase.FirstName} has been hired in restaurant {restaurantDatabase.Name}");
        }

        [HttpPost]
        [Route("employee/type")]
        public async Task<IActionResult> AddNewTypeOfEmployee(string name)
        {
            if (GeneralValidator.isEmptyNameOf(name))
            {
                return BadRequest("Employee type can't be empty");
            }

            IEnumerable<GetEmployeeTypeDTO?> allTypes = await _restaurantsApiService.GetEmployeeTypesAsync();
            if (EmployeeTypeValidator.isTypeExistInByName(allTypes, name))
            {
                return BadRequest($"Employee type {name} already exist");
            }

            bool isTypeHasBeenAdded = await _restaurantsApiService.AddNewEmployeeTypeAsync(name);
            if (!isTypeHasBeenAdded)
            {
                return BadRequest("Unable to add new type");
            }

            return Ok("New employee type has been added");
        }

        [HttpPut]
        [Route("{restaurantId}/employee/{empId}/type/{typeId}")]
        public async Task<IActionResult> UpdateEmployeeType(int empId, int typeId, int restaurantId)
        {
            //checking if ids are valid
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
            IEnumerable<GetEmployeeTypeDTO?> allTypes = await _restaurantsApiService.GetEmployeeTypesAsync();
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

            IEnumerable<EmployeesInRestaurant?> restaurantWorkers = await _restaurantsApiService.GetHiredEmployeesByRestaurantIdAsync(restaurantId);
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

            bool isEmployeeTypeChanged = await _restaurantsApiService.UpdateEmployeeTypeAsync(empId, typeId, restaurantId);
            if (!isEmployeeTypeChanged)
            {
                return BadRequest($"Unalbe to change employee type in restaurant {restaurantDatabase.Name}");
            }

            return Ok("Employee type has been updated");
        }

        [HttpPut]
        [Route("{restaurantId}")]
        public async Task<IActionResult> UpdateRestaurantData(int restaurantId, PutRestaurantDTO putRestaurantData)
        {
            if (!GeneralValidator.isCorrectId(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            if (putRestaurantData == null)
            {
                return BadRequest("Restaurant data can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(putRestaurantData.Name))
            {
                return BadRequest("Restaurant name can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(putRestaurantData.Status))
            {
                return BadRequest("Restaurant status can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(putRestaurantData.Address.City))
            {
                return BadRequest("City can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(putRestaurantData.Address.Street))
            {
                return BadRequest("Street can't be empty");
            }

            if (GeneralValidator.isEmptyNameOf(putRestaurantData.Address.BuildingNumber))
            {
                return BadRequest("Building number can't be empty");
            }

            Restaurant? restaurantDatabase = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);
            if (restaurantDatabase == null)
            {
                return NotFound($"Restaurant id={restaurantId} not found");
            }
            GetRestaurantDTO restaurantDetailsDatabase = await _restaurantsApiService.GetDetailedRestaurantDataAsync(restaurantDatabase);
            MapRestaurantDataService restaurantDataMapper = new MapRestaurantDataService(restaurantDetailsDatabase, putRestaurantData);
            Restaurant restaurantUpdatedData = restaurantDataMapper.GetRestaurantUpdatedData();

            bool isRestaurantUpdated = await _restaurantsApiService.UpdateRestaurantDataAsync(restaurantId, restaurantUpdatedData);
            if (!isRestaurantUpdated)
            {
                return BadRequest("Something went wrong unable to update restaurant data");
            }

            return Ok("Restaurant data has been updated");
        }

        [HttpPut]
        [Route("dish/{dishId}")]
        public async Task<IActionResult> UpdateDishData(int dishId, PutDishDTO putDishData)
        {
            if (!GeneralValidator.isCorrectId(dishId))
            {
                return BadRequest($"Dish id={dishId} is invalid");
            }

            if (GeneralValidator.isEmptyNameOf(putDishData.Name))
            {
                return BadRequest("Dish name can't be empty");
            }

            Dish? dishDetailsDatabase = await _restaurantsApiService.GetBasicDishDataByIdAsync(dishId);
            if (dishDetailsDatabase == null)
            {
                return NotFound($"Dish id={dishId} not found");
            }

            MapDishDataService dishDataMapper = new MapDishDataService(dishDetailsDatabase, putDishData);
            Dish dishUpdatedData = dishDataMapper.GetDishUpdatedData();

            bool isDishUpdated = await _restaurantsApiService.UpdateDishDataAsync(dishId, dishUpdatedData);
            if (!isDishUpdated)
            {
                return BadRequest("Something went wrong unable to update dish data");
            }

            return Ok("Dish data has been updated");
        }

        [HttpDelete]
        [Route("dish/{dishId}")]
        public async Task<IActionResult> DeleteDishBy(int dishId)
        {
            if (!GeneralValidator.isCorrectId(dishId))
            {
                return BadRequest($"Dish id={dishId} is invalid");
            }

            Dish? dishDatabase = await _restaurantsApiService.GetBasicDishDataByIdAsync(dishId);
            if (dishDatabase == null)
            {
                return NotFound("Dish not found");
            }

            bool isDishHasBeenRemoved = await _restaurantsApiService.DeleteDishAsync(dishDatabase);
            if (!isDishHasBeenRemoved)
            {
                return BadRequest("Something went wrong unable to delete dish");
            }

            return Ok($"Dish {dishDatabase.Name} has been deleted from all restaurants");
        }

        [HttpDelete("{restaurantId}/dish/{dishId}")]
        public async Task<IActionResult> DeleteDishbyRestaurant(int restaurantId, int dishId)
        {
            if (!GeneralValidator.isCorrectId(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            if (!GeneralValidator.isCorrectId(dishId))
            {
                return BadRequest($"Dish id={dishId} is invalid");
            }

            Dish? dishDatabase = await _restaurantsApiService.GetBasicDishDataByIdAsync(dishId);
            if (dishDatabase == null)
            {
                return NotFound("Dish not found");
            }

            Restaurant? restaurantDatabase = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);
            if (restaurantDatabase == null)
            {
                return NotFound("Restaurant not found");
            }

            IEnumerable<DishInRestaurant?> restaurantDishes = await _restaurantsApiService.GetRestaurantDishesByRestaurantIdAsync(restaurantId);
            if (restaurantDishes == null || restaurantDishes.Count() == 0)
            {
                return NotFound($"Dishes in restaurant {restaurantDatabase.Name} not found");
            }

            DishInRestaurant? restaurantDish = restaurantDishes.Where(rd => rd?.IdDish == dishId).FirstOrDefault();
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

        [HttpDelete]
        [Route("{restaurantId}/employee/{empId}")]
        public async Task<IActionResult> DeleteEmployeeFromRestaurant(int empId, int restaurantId)
        {
            if (!GeneralValidator.isCorrectId(empId))
            {
                return BadRequest($"Employee id={empId} is invalid");
            }

            if (!GeneralValidator.isCorrectId(restaurantId))
            {
                return BadRequest($"Restaurant id={restaurantId} is invalid");
            }

            Employee? employeeDatabase = await _employeeApiService.GetBasicEmployeeDataByIdAsync(empId);
            if (employeeDatabase == null)
            {
                return NotFound("Employee not found");
            }

            Restaurant? restaurantDatabase = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);
            if (restaurantDatabase == null)
            {
                return NotFound("Restaurant not found");
            }

            IEnumerable<EmployeesInRestaurant?> restaurantyWorkers = await _restaurantsApiService.GetHiredEmployeesByRestaurantIdAsync(restaurantId);
            if (restaurantyWorkers == null || restaurantyWorkers.Count() == 0)
            {
                return NotFound($"Workers in restaurant {restaurantDatabase.Name} not found");
            }

            EmployeesInRestaurant? worker = restaurantyWorkers.Where(rw => rw?.IdRestaurant == restaurantId && rw.IdEmployee == empId).FirstOrDefault();
            if (worker == null)
            {
                return NotFound($"Employee {employeeDatabase.FirstName} not found in restaurant {restaurantDatabase.Name}");
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
