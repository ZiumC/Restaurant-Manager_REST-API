﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs.GetDTO;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Services.Database_Service;
using Restaurants_REST_API.Services.ValidatorService;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/menage/[controller]")]
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
            if (id < 0)
            {
                return BadRequest($"Incorrect id, expected id grater than 0 but got {id}");
            }

            Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantDataByIdAsync(id);

            if (restaurant == null)
            {
                return NotFound($"Restaurant not found");
            }

            GetRestaurantDTO restaurantDTO = await _restaurantsApiService.GetRestaurantDetailsByIdAsync(id);

            return Ok(restaurantDTO);
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
                return BadRequest("something went wrong restaruant wasn't added");
            }
            return Ok("Restaurant has been added");
        }

        [HttpPost]
        [Route("add-dish")]
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

            //checking if type id exist
            string? typeName = "";
            IEnumerable<GetEmployeeTypeDTO?> allTypes = await _employeeApiService.GetAllEmployeeTypesAsync();
            if (allTypes == null || allTypes.Count() == 0)
            {
                return NotFound("Employee types not found");
            }
            else
            {
                bool typeExist = false;
                foreach (var type in allTypes)
                {
                    if (type != null && type.IdType == typeId)
                    {
                        typeExist = true;
                    }
                }

                if (!typeExist)
                {
                    return NotFound($"Type id={typeId} not found");
                }

                typeName = allTypes.Where(t => t?.IdType == typeId).Select(t => t?.Name).FirstOrDefault();
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
                int? empIdInRestaurantQuery = restaurantWorkers.Where(rw => rw?.IdEmployee == empId).Select(rw => rw?.IdEmployee).FirstOrDefault();
                if (empIdInRestaurantQuery != null)
                {
                    return BadRequest($"Employee {employeeDatabase.FirstName} already works in restaurant {restaurantDatabase.Name}");
                    //return NotFound($"Employee {employeeDatabase.FirstName} not found in restaurant {restaurantDatabase.Name}");
                }

                ////checking if employee is already hired as passed type id in passed restaurant id
                //int? empIdInRestaurantWithTypeQuery = restaurantWorkers.Where(rw => rw?.IdType == typeId && rw.IdEmployee == empId).Select(rw => rw?.IdEmployee).FirstOrDefault();
                //if (empIdInRestaurantWithTypeQuery != null)
                //{
                //    return BadRequest($"Employee {employeeDatabase.FirstName} has already type {typeName} in restaurant {restaurantDatabase.Name}");
                //}

                //checking if employee has more than 1 type in specyfic restaurant
                //int? empTypesCount = restaurantWorkers.Where(rw => rw?.IdEmployee == empId).ToList().Count();
                //if (empTypesCount > 0) 
                //{
                //    return BadRequest($"Employee {employeeDatabase.FirstName} already works in restaurant {restaurantDatabase.Name}");
                //}

                //checking if owner already exist 
                if (typeId == int.Parse(_config["OwnerTypeId"]))
                {
                    var ownersCount = restaurantWorkers.Where(t => t?.IdType == 1).ToList();
                    if (ownersCount != null && ownersCount.Count() >= 1)
                    {
                        return BadRequest($"Unable to add type Owner because owner already exists");
                    }
                }

            }

            ////at this stage i am certain that type name exist in database
            //string typeName = allTypes.Where(e => e.IdType == employeeHire.IdType).Select(e => e.Name).First();
            //GetRestaurantDTO restaurantDetails = await _restaurantsApiService.GetRestaurantDetailsByIdAsync(employeeHire.IdRestaurant);
            //if (EmployeeTypeValidator.isEmployeeAlreadyHasTypeIn(restaurantDetails, employeeHire.IdEmployee, typeName))
            //{
            //    return BadRequest($"Employee {existEmployee.FirstName} has already type of {typeName} in restaurant {restaurant.Name}");
            //}

            //bool isEmployeeHired = await _restaurantsApiService.HireNewEmployeeAsync(empId, typeId, restaurantId);
            //if (!isEmployeeHired)
            //{
            //    return BadRequest("Something went wrong unable to hire employee");
            //}


            return Ok($"Employee {employeeDatabase.FirstName} has been hired in {restaurantDatabase.Name} as {typeName}");
        }

    }
}
