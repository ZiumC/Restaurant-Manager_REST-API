using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
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

        public async Task<IEnumerable<GetRestaurantDTO>?> GetAllRestaurantsAsync()
        {
            return await (from rest in _context.Restaurants
                          join addr in _context.Address
                          on rest.IdAddress equals addr.IdAddress

                          select new GetRestaurantDTO
                          {
                              IdRestaurant = rest.IdRestaurant,
                              Name = rest.Name,
                              Status = rest.RestaurantStatus,
                              BonusBudget = rest.BonusBudget,
                              Address = new GetAddressDTO
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

                                                   select new GetRestaurantWorkersDTO
                                                   {
                                                       IdEmployee = eir.IdEmployee,
                                                       EmployeeType = et.Name
                                                   }
                                                   ).ToList(),

                              RestaurantReservations = (from r in _context.Reservations
                                                        where r.IdRestauration == rest.IdRestaurant

                                                        select new GetReservationDTO
                                                        {
                                                            IdReservation = r.IdReservation,
                                                            ReservationDate = r.ReservationDate,
                                                            Status = r.ReservationStatus,
                                                            ReservationGrade = r.ReservationGrade,
                                                            HowManyPeoples = r.HowManyPeoples,
                                                            ReservationComplain = (from c in _context.Complains
                                                                                   where c.IdReservation == r.IdReservation

                                                                                   select new GetComplainDTO
                                                                                   {
                                                                                       IdComplain = c.IdComplain,
                                                                                       ComplainDate = c.ComplainDate,
                                                                                       Status = c.ComplainStatus,
                                                                                       Message = c.ComplainMessage
                                                                                   }
                                                                                   ).FirstOrDefault()
                                                        }
                                                        ).ToList(),

                              RestaurantComplains = (from c in _context.Complains
                                                     where c.IdRestaurant == rest.IdRestaurant

                                                     select new GetComplainDTO
                                                     {
                                                         IdComplain = c.IdComplain,
                                                         ComplainDate = c.ComplainDate,
                                                         Status = c.ComplainStatus,
                                                         Message = c.ComplainMessage
                                                     }
                                                     ).ToList()



                          }
                          ).ToListAsync();
        }
        public async Task<GetRestaurantDTO> GetRestaurantDetailsByIdAsync(int restaurantId)
        {
            return await (from rest in _context.Restaurants
                          join addr in _context.Address
                          on rest.IdAddress equals addr.IdAddress

                          where rest.IdRestaurant == restaurantId

                          select new GetRestaurantDTO
                          {
                              IdRestaurant = rest.IdRestaurant,
                              Name = rest.Name,
                              Status = rest.RestaurantStatus,
                              BonusBudget = rest.BonusBudget,
                              Address = new GetAddressDTO
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

                                                   select new GetRestaurantWorkersDTO
                                                   {
                                                       IdEmployee = eir.IdEmployee,
                                                       EmployeeType = et.Name
                                                   }
                                                   ).ToList(),

                              RestaurantReservations = (from r in _context.Reservations
                                                        where r.IdRestauration == restaurantId

                                                        select new GetReservationDTO
                                                        {
                                                            IdReservation = r.IdReservation,
                                                            ReservationDate = r.ReservationDate,
                                                            Status = r.ReservationStatus,
                                                            ReservationGrade = r.ReservationGrade,
                                                            HowManyPeoples = r.HowManyPeoples,
                                                            ReservationComplain = (from c in _context.Complains
                                                                                   where c.IdReservation == r.IdReservation

                                                                                   select new GetComplainDTO
                                                                                   {
                                                                                       IdComplain = c.IdComplain,
                                                                                       ComplainDate = c.ComplainDate,
                                                                                       Status = c.ComplainStatus,
                                                                                       Message = c.ComplainMessage
                                                                                   }
                                                                                   ).FirstOrDefault()
                                                        }
                                                        ).ToList(),

                              RestaurantComplains = (from c in _context.Complains
                                                     where c.IdRestaurant == restaurantId

                                                     select new GetComplainDTO
                                                     {
                                                         IdComplain = c.IdComplain,
                                                         ComplainDate = c.ComplainDate,
                                                         Status = c.ComplainStatus,
                                                         Message = c.ComplainMessage
                                                     }
                                                     ).ToList()

                          }
                          ).FirstAsync();
        }

        public async Task<IEnumerable<GetReservationDTO>?> GetAllReservationsAsync()
        {
            return await (from r in _context.Reservations

                          select new GetReservationDTO
                          {
                              IdReservation = r.IdReservation,
                              ReservationDate = r.ReservationDate,
                              Status = r.ReservationStatus,
                              ReservationGrade = r.ReservationGrade,
                              HowManyPeoples = r.HowManyPeoples,

                              ReservationComplain = (from c in _context.Complains
                                                     where c.IdReservation == r.IdReservation

                                                     select new GetComplainDTO
                                                     {
                                                         IdComplain = c.IdComplain,
                                                         ComplainDate = c.ComplainDate,
                                                         Status = c.ComplainStatus,
                                                         Message = c.ComplainMessage
                                                     }
                                                   ).FirstOrDefault()
                          }

                          ).ToListAsync();
        }

        //public Task<Reservation> GetReservationsByRestaurantIdAsync(int restaurantId)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Complain> GetComplainsByRestaurantIdAsync(int restaurantId)
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Restaurant?> GetBasicRestaurantDataByIdAsync(int restaurantId)
        {
            return await _context.Restaurants.Where(e => e.IdRestaurant == restaurantId).FirstOrDefaultAsync();
        }

        public async Task<bool> AddNewRestaurantAsync(RestaurantDTO newRestaurant)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var newDatabaseAddress = _context.Address.Add
                        (
                            new Address
                            {
                                City = newRestaurant.Address.City,
                                Street = newRestaurant.Address.Street,
                                BuildingNumber = newRestaurant.Address.BuildingNumber,
                                LocalNumber = newRestaurant.Address.LocalNumber
                            }
                    );
                    await _context.SaveChangesAsync();

                    var newDatabaseRestaurant = _context.Restaurants.Add
                        (
                           new Restaurant
                           {
                               Name = newRestaurant.Name,
                               RestaurantStatus = newRestaurant.Status,
                               BonusBudget = newRestaurant.BonusBudget,
                               IdAddress = newDatabaseAddress.Entity.IdAddress
                           }
                        );
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    await transaction.RollbackAsync();
                    return false;
                }

                await transaction.CommitAsync();
                return true;
            }
        }

        public async Task<bool> AddNewDishToRestaurantsAsync(DishDTO newDish)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var newDatabaseDish = _context.Dishes.Add
                        (
                            new Dish
                            {
                                Name = newDish.Name,
                                Price = newDish.Price
                            }
                        );
                    await _context.SaveChangesAsync();

                    foreach (int idRestaurant in newDish.IdRestaurants)
                    {
                        var newDishInRestaurant = _context.RestaurantDishes.Add
                            (
                                    new DishInRestaurant
                                    {
                                        IdDish = newDatabaseDish.Entity.IdDish,
                                        IdRestaurant = idRestaurant
                                    }
                            );
                        await _context.SaveChangesAsync();
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    await transaction.RollbackAsync();
                    return false;
                }

                await transaction.CommitAsync();
                return true;
            }
        }

        public async Task<bool> HireNewEmployeeAsync(EmployeeToRestaurantDTO employeeHire)
        {
            try
            {
                var newDatabaseEmployeeHired = _context.EmployeesInRestaurants.Add
                    (
                        new EmployeesInRestaurant
                        {
                            IdEmployee = employeeHire.IdEmployee,
                            IdRestaurant = employeeHire.IdRestaurant,
                            IdType = employeeHire.IdEmployeeType
                        }
                    );
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
    }
}
