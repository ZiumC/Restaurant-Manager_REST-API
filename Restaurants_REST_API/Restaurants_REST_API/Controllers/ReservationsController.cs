﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Models;
using Restaurants_REST_API.Services.Database_Service;

namespace Restaurants_REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IRestaurantApiService _restaurantsApiService;
        private readonly IReservationApiService _reservationsApiService;

        public ReservationsController(IRestaurantApiService restaurantsApiService, IReservationApiService reservationsApiService)
        {
            _restaurantsApiService = restaurantsApiService;
            _reservationsApiService = reservationsApiService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReservations()
        {
            var reservations = await _reservationsApiService.GetAllReservationsAsync();

            if (reservations == null)
            {
                return NotFound($"Reservations not found");
            }

            return Ok(reservations);
        }

        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetReservationBy(int id)
        {
            if (id < 0)
            {
                return BadRequest($"Incorrect id, expected id grater than 0 but got {id}");
            }

            ReservationDTO? reservation = await _reservationsApiService.GetReservationByIdAsync(id);

            if (reservation == null)
            {
                return NotFound($"Reservation {id} not found");
            }

            return Ok(reservation);
        }

        [HttpGet]
        [Route("restaurant/id")]
        public async Task<IActionResult> GetReservationsByRestaurant(int id)
        {
            if (id < 0)
            {
                return BadRequest($"Incorrect id, expected id grater than 0 but got {id}");
            }

            Restaurant? restaurant = await _restaurantsApiService.GetBasicRestaurantInfoByIdAsync(id);

            if (restaurant == null)
            {
                return NotFound($"Restaurant {id} not found");
            }

            var reservations = await _reservationsApiService.GetReservationsByRestaurantIdAsync(id);

            if (reservations == null || reservations.Count() == 0)
            {
                return NotFound("Reservations not found");
            }

            return Ok(reservations);
        }

        [HttpGet]
        [Route("client/id")]
        public async Task<IActionResult> GetReservationsByClient(int id)
        {
            if (id < 0)
            {
                return BadRequest($"Incorrect id, expected id grater than 0 but got {id}");
            }

            ClientDTO? client = await _reservationsApiService.GetReservationsByClientIdAsync(id);

            if (client == null)
            {
                return NotFound($"Client {id} not found");
            }

            return Ok(client);
        }

    }
}