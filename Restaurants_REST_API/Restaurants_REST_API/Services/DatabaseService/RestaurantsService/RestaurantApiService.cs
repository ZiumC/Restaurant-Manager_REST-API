using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class RestaurantApiService : IRestaurantApiService
    {

        private readonly MainDbContext _context;

        public RestaurantApiService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RestaurantDTO>?> GetAllRestaurantsAsync()
        {
            return await (from rest in _context.Restaurants
                          join addr in _context.Address
                          on rest.IdAddress equals addr.IdAddress

                          select new RestaurantDTO
                          {
                              IdRestaurant = rest.IdRestaurant,
                              Name = rest.Name,
                              Status = rest.RestaurantStatus,
                              BonusBudget = rest.BonusBudget,
                              Address = new AddressDTO
                              {
                                  IdAddress = addr.IdAddress,
                                  City = addr.City,
                                  Street = addr.Street,
                                  BuildingNumber = addr.BuildingNumber,
                                  LocalNumber = addr.LocalNumber
                              },

                              RestaurantDishes = (from rd in _context.RestaurantDishes
                                                  join d in _context.Dishes
                                                  on rd.IdDish equals d.IdDish

                                                  where rd.IdRestaurant == rest.IdRestaurant

                                                  select new Dish
                                                  {
                                                      IdDish = d.IdDish,
                                                      Name = d.Name,
                                                      Price = d.Price,
                                                  }
                                                  ).ToList(),

                              RestaurantWorkers = (from eir in _context.EmployeesInRestaurants
                                                   join et in _context.EmployeeTypes
                                                   on eir.IdType equals et.IdType

                                                   where eir.IdRestaurant == rest.IdRestaurant

                                                   select new RestaurantWorersDTO
                                                   {
                                                       IdEmployee = eir.IdEmployee,
                                                       EmployeeType = et.Name
                                                   }
                                                   ).ToList(),

                              RestaurantReservations = (from r in _context.Reservations
                                                        where r.IdRestauration == rest.IdRestaurant

                                                        select new ReservationDTO
                                                        {
                                                            IdReservation = r.IdReservation,
                                                            ReservationDate = r.ReservationDate,
                                                            Status = r.ReservationStatus,
                                                            ReservationGrade = r.ReservationGrade,
                                                            TableNumber = r.TableNumber,
                                                            ReservationComplain = (from c in _context.Complains
                                                                                   where c.IdReservation == r.IdReservation

                                                                                   select new ComplainDTO
                                                                                   {
                                                                                       IdComplain = c.IdComplain,
                                                                                       ComplainDate = c.ComplainDate,
                                                                                       Status = c.ComplainStatus
                                                                                   }
                                                                                   ).FirstOrDefault()
                                                        }
                                                        ).ToList(),

                              RestaurantComplains = (from c in _context.Complains
                                                     where c.IdRestaurant == rest.IdRestaurant

                                                     select new ComplainDTO
                                                     {
                                                         IdComplain = c.IdComplain,
                                                         ComplainDate = c.ComplainDate,
                                                         Status = c.ComplainStatus
                                                     }
                                                     ).ToList()



                          }
                          ).ToListAsync();
        }
        public async Task<RestaurantDTO> GetRestaurantDetailsByIdAsync(int restaurantId)
        {
            return await (from rest in _context.Restaurants
                          join addr in _context.Address
                          on rest.IdAddress equals addr.IdAddress

                          where rest.IdRestaurant == restaurantId

                          select new RestaurantDTO
                          {
                              IdRestaurant = rest.IdRestaurant,
                              Name = rest.Name,
                              Status = rest.RestaurantStatus,
                              BonusBudget = rest.BonusBudget,
                              Address = new AddressDTO
                              {
                                  IdAddress = addr.IdAddress,
                                  City = addr.City,
                                  Street = addr.Street,
                                  BuildingNumber = addr.BuildingNumber,
                                  LocalNumber = addr.LocalNumber
                              },

                              RestaurantDishes = (from rd in _context.RestaurantDishes
                                                  join d in _context.Dishes
                                                  on rd.IdDish equals d.IdDish

                                                  where rd.IdRestaurant == restaurantId

                                                  select new Dish
                                                  {
                                                      IdDish = d.IdDish,
                                                      Name = d.Name,
                                                      Price = d.Price,
                                                  }
                                                  ).ToList(),

                              RestaurantWorkers = (from eir in _context.EmployeesInRestaurants
                                                   join et in _context.EmployeeTypes
                                                   on eir.IdType equals et.IdType

                                                   where eir.IdRestaurant == restaurantId

                                                   select new RestaurantWorersDTO
                                                   {
                                                       IdEmployee = eir.IdEmployee,
                                                       EmployeeType = et.Name
                                                   }
                                                   ).ToList(),

                              RestaurantReservations = (from r in _context.Reservations
                                                        where r.IdRestauration == restaurantId

                                                        select new ReservationDTO
                                                        {
                                                            IdReservation = r.IdReservation,
                                                            ReservationDate = r.ReservationDate,
                                                            Status = r.ReservationStatus,
                                                            ReservationGrade = r.ReservationGrade,
                                                            TableNumber = r.TableNumber,
                                                            ReservationComplain = (from c in _context.Complains
                                                                                   where c.IdReservation == r.IdReservation

                                                                                   select new ComplainDTO
                                                                                   {
                                                                                       IdComplain = c.IdComplain,
                                                                                       ComplainDate = c.ComplainDate,
                                                                                       Status = c.ComplainStatus
                                                                                   }
                                                                                   ).FirstOrDefault()
                                                        }
                                                        ).ToList(),

                              RestaurantComplains = (from c in _context.Complains
                                                     where c.IdRestaurant == restaurantId

                                                     select new ComplainDTO
                                                     {
                                                         IdComplain = c.IdComplain,
                                                         ComplainDate = c.ComplainDate,
                                                         Status = c.ComplainStatus
                                                     }
                                                     ).ToList()

                          }
                          ).FirstAsync();
        }

        public async Task<IEnumerable<ReservationDTO>?> GetAllReservationsAsync()
        {
            return await (from r in _context.Reservations

                          select new ReservationDTO
                          {
                              IdReservation = r.IdReservation,
                              ReservationDate = r.ReservationDate,
                              Status = r.ReservationStatus,
                              ReservationGrade = r.ReservationGrade,
                              TableNumber = r.TableNumber,

                              ReservationComplain = (from c in _context.Complains
                                                     where c.IdReservation == r.IdReservation

                                                     select new ComplainDTO
                                                     {
                                                         IdComplain = c.IdComplain,
                                                         ComplainDate = c.ComplainDate,
                                                         Status = c.ComplainStatus
                                                     }
                                                   ).FirstOrDefault()
                          }

                          ).ToListAsync();
        }

        public Task<Reservation> GetReservationsByRestaurantIdAsync(int restaurantId)
        {
            throw new NotImplementedException();
        }

        public Task<Complain> GetComplainsByRestaurantIdAsync(int restaurantId)
        {
            throw new NotImplementedException();
        }

        public async Task<Restaurant?> GetBasicRestaurantInfoByIdAsync(int restaurantId)
        {
            return await _context.Restaurants.Where(e => e.IdRestaurant == restaurantId).FirstOrDefaultAsync();
        }
    }
}
