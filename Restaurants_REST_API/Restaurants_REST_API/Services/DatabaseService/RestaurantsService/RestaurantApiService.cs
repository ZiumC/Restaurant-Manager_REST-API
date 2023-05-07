using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTO;
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

        public async Task<IEnumerable<GetRestaurantDTO?>> GetAllRestaurantsAsync()
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
                                                  }).ToList(),

                              RestaurantWorkers = (from eir in _context.EmployeesInRestaurants
                                                   join et in _context.EmployeeTypes
                                                   on eir.IdType equals et.IdType

                                                   where eir.IdRestaurant == rest.IdRestaurant

                                                   select new GetRestaurantWorkersDTO
                                                   {
                                                       IdEmployee = eir.IdEmployee,
                                                       EmployeeType = et.Name
                                                   }).ToList(),

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
                                                                                   }).FirstOrDefault()
                                                        }).ToList(),

                              RestaurantComplains = (from c in _context.Complains
                                                     where c.IdRestaurant == rest.IdRestaurant

                                                     select new GetComplainDTO
                                                     {
                                                         IdComplain = c.IdComplain,
                                                         ComplainDate = c.ComplainDate,
                                                         Status = c.ComplainStatus,
                                                         Message = c.ComplainMessage
                                                     }).ToList()



                          }).ToListAsync();
        }

        public async Task<GetRestaurantDTO> GetDetailedRestaurantDataAsync(Restaurant restaurant)
        {
            int restaurantId = restaurant.IdRestaurant;

            var getRestaurantAddressQuery = await
                (from rest in _context.Restaurants
                 join addr in _context.Address
                 on rest.IdAddress equals addr.IdAddress

                 where rest.IdRestaurant == restaurantId

                 select new GetAddressDTO
                 {
                     IdAddress = addr.IdAddress,
                     City = addr.City,
                     Street = addr.Street,
                     BuildingNumber = addr.BuildingNumber,
                     LocalNumber = addr.LocalNumber
                 }).FirstAsync();

            var getRestaurantDishesQuery = await
                (from rd in _context.RestaurantDishes
                 join d in _context.Dishes
                 on rd.IdDish equals d.IdDish

                 where rd.IdRestaurant == restaurantId

                 select new Dish
                 {
                     IdDish = d.IdDish,
                     Name = d.Name,
                     Price = d.Price,
                 }).ToListAsync();

            var getRestaurantWorkersQuery = await
                (from eir in _context.EmployeesInRestaurants
                 join et in _context.EmployeeTypes
                 on eir.IdType equals et.IdType

                 where eir.IdRestaurant == restaurantId

                 select new GetRestaurantWorkersDTO
                 {
                     IdEmployee = eir.IdEmployee,
                     EmployeeType = et.Name
                 }).ToListAsync();

            var getRestaurantComplainsQuery = await
                (from c in _context.Complains
                 where c.IdRestaurant == restaurantId

                 select new GetComplainDTO
                 {
                     IdComplain = c.IdComplain,
                     ComplainDate = c.ComplainDate,
                     Status = c.ComplainStatus,
                     Message = c.ComplainMessage
                 }).ToListAsync();

            var getRestaurantReservationsQuery = await
                (from r in _context.Reservations
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
                                            }).FirstOrDefault()
                 }).ToListAsync();

            return new GetRestaurantDTO
            {
                IdRestaurant = restaurant.IdRestaurant,
                Name = restaurant.Name,
                Status = restaurant.RestaurantStatus,
                BonusBudget = restaurant.BonusBudget,


                Address = getRestaurantAddressQuery,
                RestaurantWorkers = getRestaurantWorkersQuery,
                RestaurantDishes = getRestaurantDishesQuery,
                RestaurantReservations = getRestaurantReservationsQuery,
                RestaurantComplains = getRestaurantComplainsQuery
            };
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
                                                     }).FirstOrDefault()
                          }).ToListAsync();
        }

        public async Task<Restaurant?> GetBasicRestaurantDataByIdAsync(int restaurantId)
        {
            return await _context.Restaurants
                .Where(e => e.IdRestaurant == restaurantId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmployeesInRestaurant?>> GetHiredEmployeesInRestaurantsAsync()
        {
            return await _context.EmployeesInRestaurants
                .Select(eir => new EmployeesInRestaurant
                {
                    IdRestaurantWorker = eir.IdRestaurantWorker,
                    IdEmployee = eir.IdEmployee,
                    IdRestaurant = eir.IdRestaurant,
                    IdType = eir.IdType
                }).ToListAsync();
        }

        public async Task<IEnumerable<GetEmployeeTypeDTO?>> GetEmployeeTypesAsync()
        {
            return await _context.EmployeeTypes
                .Select(x => new GetEmployeeTypeDTO
                {
                    IdType = x.IdType,
                    Name = x.Name
                }).ToListAsync();
        }

        public async Task<Dish?> GetBasicDishDataByIdAsync(int dishId)
        {
            return await
                (from d in _context.Dishes
                 where d.IdDish == dishId

                 select new Dish
                 {
                     IdDish = dishId,
                     Name = d.Name,
                     Price = d.Price
                 }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<DishInRestaurant?>> GetRestaurantDishesByRestaurantIdAsync(int restaurantId)
        {
            return await
                (_context.RestaurantDishes
                .Where(rd => rd.IdRestaurant == restaurantId))
                .ToListAsync();
        }

        public async Task<bool> AddNewEmployeeTypeAsync(string name)
        {
            try
            {
                var newType = _context.Add
                (
                    new EmployeeType
                    {
                        Name = name
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

        public async Task<bool> AddNewRestaurantAsync(PostRestaurantDTO newRestaurant)
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

        public async Task<bool> AddNewDishToRestaurantsAsync(PostDishDTO newDish)
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

        public async Task<bool> AddNewEmployeeToRestaurantAsync(int empId, int typeId, int restaurantId)
        {
            try
            {
                var newDatabaseEmployeeHired = _context.EmployeesInRestaurants.Add
                    (
                        new EmployeesInRestaurant
                        {
                            IdEmployee = empId,
                            IdRestaurant = restaurantId,
                            IdType = typeId
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

        public async Task<bool> UpdateRestaurantDataAsync(int restaurantId, Restaurant newRestaurantData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var updateRestaurantDataQuery = await
                        (_context.Restaurants.Where(r => r.IdRestaurant == restaurantId)).FirstAsync();

                    updateRestaurantDataQuery.Name = newRestaurantData.Name;
                    updateRestaurantDataQuery.RestaurantStatus = newRestaurantData.RestaurantStatus;
                    updateRestaurantDataQuery.BonusBudget = newRestaurantData.BonusBudget;

                    await _context.SaveChangesAsync();


                    var updateRestaurantAddressQuery = await
                        (_context.Address.Where(a => a.IdAddress == newRestaurantData.Address.IdAddress)).FirstAsync();

                    updateRestaurantAddressQuery.City = newRestaurantData.Address.City;
                    updateRestaurantAddressQuery.Street = newRestaurantData.Address.Street;
                    updateRestaurantAddressQuery.BuildingNumber = newRestaurantData.Address.BuildingNumber;
                    updateRestaurantAddressQuery.LocalNumber = newRestaurantData.Address.LocalNumber;

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

        public async Task<bool> UpdateDishDataAsync(int dishId, Dish newDishData)
        {
            try
            {
                var updateDishDataQuery = await
                    (_context.Dishes
                    .Where(d => d.IdDish == dishId)
                    .FirstAsync());

                updateDishDataQuery.Name = newDishData.Name;
                updateDishDataQuery.Price = newDishData.Price;

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateEmployeeTypeAsync(int empId, int typeId, int restaurantId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var updateEmpTypeQuery = await
                        (_context.EmployeesInRestaurants
                        .Where(eir => eir.IdEmployee == empId && eir.IdRestaurant == restaurantId)
                        .FirstAsync());

                    updateEmpTypeQuery.IdType = typeId;

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

        public async Task<bool> DeleteDishAsync(Dish dishData)
        {
            try
            {
                //removing dish
                _context.Remove(dishData);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteDishFromRestaurantAsync(int restaurantId, int dishId)
        {
            try
            {
                var deleteDishFromRestaurantQuery = await
                    (_context.RestaurantDishes
                    .Where(rd => rd.IdRestaurant == restaurantId && rd.IdDish == dishId)
                    .FirstAsync()
                    );

                _context.Remove(deleteDishFromRestaurantQuery);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }

        public async Task<bool> DeleteEmployeeFromRestaurantAsync(int empId, int restaurantId)
        {
            try
            {
                var deleteEmployeeFromRestaurantQuery = await
                    (_context.EmployeesInRestaurants
                    .Where(eir => eir.IdRestaurant == restaurantId && eir.IdEmployee == empId)
                    .FirstAsync());

                _context.Remove(deleteEmployeeFromRestaurantQuery);
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
