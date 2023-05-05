﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetRestaurantBy(int id)
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

            GetRestaurantDTO restaurantDTO = await _restaurantsApiService.GetRestaurantDetailedDataAsync(restaurant);

            return Ok(restaurantDTO);
        }
        [HttpGet]
        [Route("dish/id")]
        public async Task<IActionResult> GetDishBy(int id)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Dish id={id} is invalid");
            }

            GetDishDTO? dishDetailsData = await _restaurantsApiService.GetDishDetailsByIdAsync(id);
            if (dishDetailsData == null)
            {
                return NotFound($"Dish id={id} not found");
            }

            return Ok(dishDetailsData);
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

            if (newDish.IdRestaurants.Count() > 0)
            {
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
                        return NotFound($"Given restaurant id={restaurantId} is invalid");
                    }

                    restaurants.Add(await _restaurantsApiService.GetRestaurantDetailedDataAsync(restaurant));
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
        [Route("hire-employee")]
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
                return NotFound($"Employee id={empId} not found");
            }

            //checking if restaurant exist
            Restaurant? restaurantDatabase = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(restaurantId);
            if (restaurantDatabase == null)
            {
                return NotFound($"Restaurant id={restaurantId} not found");
            }

            IEnumerable<EmployeesInRestaurant?> restaurantWorkers = await _restaurantsApiService.GetEmployeeInRestaurantDataByRestaurantIdAsync(restaurantId);
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

            bool isEmployeeHired = await _restaurantsApiService.HireNewEmployeeAsync(empId, typeId, restaurantId);
            if (!isEmployeeHired)
            {
                return BadRequest("Something went wrong unable to hire employee");
            }

            return Ok($"Employee {employeeDatabase.FirstName} has been hired in restaurant {restaurantDatabase.Name}");
        }

        [HttpPost]
        [Route("new-type")]
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
        [Route("update-emp-type")]
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

        [HttpPut]
        [Route("id")]
        public async Task<IActionResult> UpdateRestaurantData(int id, PutRestaurantDTO putRestaurantData)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Restaurant id={id} is invalid");
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

            Restaurant? restaurantDatabase = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(id);
            if (restaurantDatabase == null)
            {
                return NotFound($"Restaurant id={id} not found");
            }
            GetRestaurantDTO restaurantDetailsDatabase = await _restaurantsApiService.GetRestaurantDetailedDataAsync(restaurantDatabase);
            MapRestaurantDataService restaurantDataMapper = new MapRestaurantDataService(restaurantDetailsDatabase, putRestaurantData);
            Restaurant restaurantUpdatedData = restaurantDataMapper.GetRestaurantUpdatedData();

            bool isRestaurantUpdated = await _restaurantsApiService.UpdateRestaurantDataAsync(id, restaurantUpdatedData);
            if (!isRestaurantUpdated)
            {
                return BadRequest("Something went wrong unable to update restaurant data");
            }

            return Ok("Restaurant data has been updated");
        }

        [HttpPut]
        [Route("dish/id")]
        public async Task<IActionResult> UpdateDishData(int id, PutDishDTO putDishData)
        {
            if (!GeneralValidator.isCorrectId(id))
            {
                return BadRequest($"Dish id={id} is invalid");
            }

            if (GeneralValidator.isEmptyNameOf(putDishData.Name))
            {
                return BadRequest("Dish name can't be empty");
            }

            GetDishDTO? dishDetailsDatabase = await _restaurantsApiService.GetDishDetailsByIdAsync(id);
            if (dishDetailsDatabase == null)
            {
                return NotFound($"Dish id={id} not found");
            }

            MapDishDataService dishDataMapper = new MapDishDataService(dishDetailsDatabase, putDishData);
            Dish dishUpdatedData = dishDataMapper.GetDishUpdatedData();

            bool isDishUpdated = await _restaurantsApiService.UpdateDishDataAsync(id, dishUpdatedData);
            if (!isDishUpdated)
            {
                return BadRequest("Something went wrong unable to update dish data");
            }

            return Ok("Dish data has been updated");
        }

    }
}
