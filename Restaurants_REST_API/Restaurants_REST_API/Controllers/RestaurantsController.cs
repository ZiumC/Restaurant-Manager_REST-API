using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.ValidatorService;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        private readonly IRestaurantApiService _restaurantsApiService;
        private readonly IEmployeeApiService _employeeApiService;

        public RestaurantsController(IRestaurantApiService restaurantsApiService, IEmployeeApiService employeeApiService)
        {
            _restaurantsApiService = restaurantsApiService;
            _employeeApiService = employeeApiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRestaurants()
        {
            IEnumerable<RestaurantDTO?> restaurants = await _restaurantsApiService.GetAllRestaurantsAsync();

            if (restaurants == null || restaurants.Count() == 0)
            {
                return NotFound("Restaurants not found");
            }

            return Ok(restaurants);
        }

        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetRestaurantBy(int id)
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

            RestaurantDTO restaurantDTO = await _restaurantsApiService.GetRestaurantDetailsByIdAsync(id);

            return Ok(restaurantDTO);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddNewRestaurant(RestaurantDTO newRestaurant)
        {
            if (newRestaurant == null)
            {
                return BadRequest("Restaurant must be specified");
            }

            if (RestaurantValidator.isEmptyNameOf(newRestaurant.Name))
            {
                return BadRequest("Restaurant name can't be empty");
            }

            if (AddressValidator.isEmptyAddressOf(newRestaurant.Address))
            {
                return BadRequest("Restaurant adress can't be empty");
            }

            if (RestaurantValidator.isEmptyNameOf(newRestaurant.Status))
            {
                return BadRequest("Restaurant statuc can't be empty");
            }

            IEnumerable<RestaurantDTO?> allRestaurants = await _restaurantsApiService.GetAllRestaurantsAsync();
            if (RestaurantValidator.isRestaurantExistIn(allRestaurants, newRestaurant))
            {
                return BadRequest("Restaurant already exist");
            }

            bool isRestaurantAdded = await _restaurantsApiService.AddNewRestaurantAsync(newRestaurant);
            if (!isRestaurantAdded)
            {
                return BadRequest("something went wrong restaruant wasn't added");
            }
            return Ok("Restaurant has been added");
        }

        [HttpPost]
        [Route("add-dish")]
        public async Task<IActionResult> AddNewDish(DishDTO newDish)
        {
            if (newDish == null)
            {
                return BadRequest("Dish must be specified");
            }

            if (newDish.IdRestaurants == null)
            {
                return BadRequest("Dish must have specified one or more restaurants");
            }

            if (RestaurantValidator.isEmptyNameOf(newDish.Name))
            {
                return BadRequest("Dish name can't be empty");
            }

            if (newDish.IdRestaurants.Count() > 0)
            {
                List<RestaurantDTO> restaurants = new List<RestaurantDTO>();

                foreach (int idRestaurant in newDish.IdRestaurants)
                {
                    if (!GeneralValidator.isCorrectId(idRestaurant))
                    {
                        return BadRequest("One or many restaurant id isn't correct");
                    }

                    Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(idRestaurant);

                    if (restaurant == null)
                    {
                        return NotFound($"Given restaurant id={idRestaurant} is invalid");
                    }

                    restaurants.Add(await _restaurantsApiService.GetRestaurantDetailsByIdAsync(idRestaurant));
                }

                if (RestaurantValidator.isDishExistIn(restaurants, newDish))
                {
                    return BadRequest("Dish already exist in one or many restaurants");
                }
            }

            bool isDishAdded = await _restaurantsApiService.AddNewDishToRestaurantsAsync(newDish);
            if (!isDishAdded)
            {
                return BadRequest("Something went wrong unable to add new dish");
            }

            return Ok("Dish has been added");
        }

        [HttpPost]
        [Route("add-emp")]
        public async Task<IActionResult> AddNewEmployeeToRestaurant(EmployeeHiredDTO employeeHire)
        {
            if (employeeHire == null)
            {
                return BadRequest("Data can't be empty");
            }

            if (!GeneralValidator.isCorrectId(employeeHire.IdEmployee))
            {
                return BadRequest("Employee id isn't correct");
            }

            if (!GeneralValidator.isCorrectId(employeeHire.IdRestaurant))
            {
                return BadRequest("Restaurant id isn't correct");
            }

            if (!GeneralValidator.isCorrectId(employeeHire.IdEmployeeType))
            {
                return BadRequest("Restaurant id isn't correct");
            }

            Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(employeeHire.IdRestaurant);
            if (restaurant == null)
            {
                return NotFound($"Restaurant id={employeeHire.IdRestaurant} doesn't exist");
            }

            Employee? existEmployee = await _employeeApiService.GetBasicEmployeeDataByIdAsync(employeeHire.IdEmployee);
            if (existEmployee == null)
            {
                return NotFound($"Employee id={employeeHire.IdEmployee} doesn't exist");
            }

            IEnumerable<EmployeeType?> allTypes = await _employeeApiService.GetAllEmployeeTypesAsync();
            if (allTypes == null || allTypes.Count() < 0)
            {
                return NotFound("Employee types doesn't found");
            }

            if (!EmployeeTypeValidator.isCorrectEmployeeTypeOf(employeeHire.IdEmployeeType, allTypes))
            {
                return NotFound("Employee type doesn't exist");
            }

            bool isEmployeeHired = await _restaurantsApiService.HireNewEmployeeAsync(employeeHire);
            if (!isEmployeeHired)
            {
                return BadRequest("Something went wrong unable to hire employee");
            }

            return Ok($"Employee has been hired as {employeeHire.IdEmployeeType}");
        }

    }
}
