using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.DTOs.GetDTO;
using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Utils.MapperService;

namespace Restaurants_REST_API.Services.Database_Service
{
    public class RestaurantApiService : IRestaurantApiService
    {

        private readonly MainDbContext _context;
        private readonly IConfiguration _config;

        public RestaurantApiService(MainDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IEnumerable<GetRestaurantDTO>?> GetAllRestaurantsAsync()
        {
            return await
                (from rest in _context.Restaurant
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

                     RestaurantDishes = (from rd in _context.RestaurantDish
                                         join d in _context.Dish
                                         on rd.IdDish equals d.IdDish

                                         where rd.IdRestaurant == rest.IdRestaurant

                                         select new Dish
                                         {
                                             IdDish = d.IdDish,
                                             Name = d.Name,
                                             Price = d.Price,
                                         }).ToList(),

                     RestaurantWorkers = (from eir in _context.EmployeeRestaurant
                                          join et in _context.EmployeeType
                                          on eir.IdType equals et.IdType

                                          join e in _context.Employee
                                          on eir.IdEmployee equals e.IdEmployee

                                          where eir.IdRestaurant == rest.IdRestaurant

                                          select new GetRestaurantWorkersDTO
                                          {
                                              IdEmployee = eir.IdEmployee,
                                              FirstName = e.FirstName,
                                              LastName = e.LastName,
                                              EmployeeType = et.Name
                                          }).ToList(),

                     RestaurantReservations = (from r in _context.Reservation
                                               where r.IdRestaurant == rest.IdRestaurant

                                               select new GetReservationDTO
                                               {
                                                   IdReservation = r.IdReservation,
                                                   ReservationDate = r.ReservationDate,
                                                   Status = r.ReservationStatus,
                                                   ReservationGrade = r.ReservationGrade,
                                                   HowManyPeoples = r.HowManyPeoples,
                                                   ReservationComplaint = (from c in _context.Complaint
                                                                           where c.IdReservation == r.IdReservation

                                                                           select new GetComplaintDTO
                                                                           {
                                                                               IdComplaint = c.IdComplaint,
                                                                               ComplaintDate = c.ComplainDate,
                                                                               Status = c.ComplaintStatus,
                                                                               Message = c.ComplaintMessage
                                                                           }).FirstOrDefault()
                                               }).ToList(),
                 }).ToListAsync();
        }

        public async Task<GetRestaurantDTO?> GetRestaurantDetailedDataAsync(int restaurantId)
        {
            var getRestaurantDataQuery =
                await _context.Restaurant
                .Where(r => r.IdRestaurant == restaurantId)
                .FirstOrDefaultAsync();

            if (getRestaurantDataQuery == null)
            {
                return null;
            }

            var getRestaurantAddressQuery = await
                (from rest in _context.Restaurant
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
                (from rd in _context.RestaurantDish
                 join d in _context.Dish
                 on rd.IdDish equals d.IdDish

                 where rd.IdRestaurant == restaurantId

                 select new Dish
                 {
                     IdDish = d.IdDish,
                     Name = d.Name,
                     Price = d.Price,
                 }).ToListAsync();

            var getRestaurantWorkersQuery = await
                (from eir in _context.EmployeeRestaurant
                 join et in _context.EmployeeType
                 on eir.IdType equals et.IdType

                 join e in _context.Employee
                 on eir.IdEmployee equals e.IdEmployee

                 where eir.IdRestaurant == restaurantId

                 select new GetRestaurantWorkersDTO
                 {
                     IdEmployee = eir.IdEmployee,
                     FirstName = e.FirstName,
                     LastName = e.LastName,
                     EmployeeType = et.Name
                 }).ToListAsync();

            var getRestaurantComplainsQuery = await
                (from c in _context.Complaint
                 where c.IdRestaurant == restaurantId

                 select new GetComplaintDTO
                 {
                     IdComplaint = c.IdComplaint,
                     ComplaintDate = c.ComplainDate,
                     Status = c.ComplaintStatus,
                     Message = c.ComplaintMessage
                 }).ToListAsync();

            var getRestaurantReservationsQuery = await
                (from r in _context.Reservation
                 where r.IdRestaurant == restaurantId

                 select new GetReservationDTO
                 {
                     IdReservation = r.IdReservation,
                     ReservationDate = r.ReservationDate,
                     Status = r.ReservationStatus,
                     ReservationGrade = r.ReservationGrade,
                     HowManyPeoples = r.HowManyPeoples,
                     ReservationComplaint = (from c in _context.Complaint
                                             where c.IdReservation == r.IdReservation

                                             select new GetComplaintDTO
                                             {
                                                 IdComplaint = c.IdComplaint,
                                                 ComplaintDate = c.ComplainDate,
                                                 Status = c.ComplaintStatus,
                                                 Message = c.ComplaintMessage
                                             }).FirstOrDefault()
                 }).ToListAsync();

            return new GetRestaurantDTO
            {
                IdRestaurant = restaurantId,
                Name = getRestaurantDataQuery.Name,
                Status = getRestaurantDataQuery.RestaurantStatus,
                BonusBudget = getRestaurantDataQuery.BonusBudget,
                Address = getRestaurantAddressQuery,
                RestaurantWorkers = getRestaurantWorkersQuery,
                RestaurantDishes = getRestaurantDishesQuery,
                RestaurantReservations = getRestaurantReservationsQuery
            };
        }

        public async Task<IEnumerable<GetReservationDTO>?> GetAllReservationsAsync()
        {
            return await
                (from r in _context.Reservation
                 select new GetReservationDTO
                 {
                     IdReservation = r.IdReservation,
                     ReservationDate = r.ReservationDate,
                     Status = r.ReservationStatus,
                     ReservationGrade = r.ReservationGrade,
                     HowManyPeoples = r.HowManyPeoples,

                     ReservationComplaint = (from c in _context.Complaint
                                             where c.IdReservation == r.IdReservation

                                             select new GetComplaintDTO
                                             {
                                                 IdComplaint = c.IdComplaint,
                                                 ComplaintDate = c.ComplainDate,
                                                 Status = c.ComplaintStatus,
                                                 Message = c.ComplaintMessage
                                             }).FirstOrDefault()
                 }).ToListAsync();
        }

        public async Task<Restaurant?> GetRestaurantSimpleDataByIdAsync(int restaurantId)
        {
            return await _context.Restaurant
                .Where(e => e.IdRestaurant == restaurantId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EmployeeRestaurant>?> GetHiredEmployeesInRestaurantsAsync()
        {
            return await _context.EmployeeRestaurant
                .Select(eir => new EmployeeRestaurant
                {
                    IdRestaurantWorker = eir.IdRestaurantWorker,
                    IdEmployee = eir.IdEmployee,
                    IdRestaurant = eir.IdRestaurant,
                    IdType = eir.IdType
                }).ToListAsync();
        }

        public async Task<IEnumerable<GetEmployeeTypeDTO>?> GetEmployeeTypesAsync()
        {
            return await _context.EmployeeType
                .Select(x => new GetEmployeeTypeDTO
                {
                    IdType = x.IdType,
                    Name = x.Name
                }).ToListAsync();
        }

        public async Task<Dish?> GetDishSimpleDataByIdAsync(int dishId)
        {
            return await
                (from d in _context.Dish
                 where d.IdDish == dishId

                 select new Dish
                 {
                     IdDish = dishId,
                     Name = d.Name,
                     Price = d.Price
                 }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RestaurantDish>?> GetRestaurantDishesByRestaurantIdAsync(int restaurantId)
        {
            return await _context.RestaurantDish
                .Where(rd => rd.IdRestaurant == restaurantId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Dish>?> GetAllDishes()
        {
            return await _context.Dish
                .ToListAsync();
        }

        public async Task<bool> AddNewRestaurantAsync(PostRestaurantDTO newRestaurantData, int ownerTypeId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var newAddressQuery = _context.Address.Add
                        (
                            new Address
                            {
                                City = newRestaurantData.Address.City,
                                Street = newRestaurantData.Address.Street,
                                BuildingNumber = newRestaurantData.Address.BuildingNumber,
                                LocalNumber = newRestaurantData.Address.LocalNumber
                            }
                    );
                    await _context.SaveChangesAsync();

                    var newRestaurantQuery = _context.Restaurant.Add
                        (
                           new Restaurant
                           {
                               Name = newRestaurantData.Name,
                               RestaurantStatus = newRestaurantData.Status,
                               BonusBudget = newRestaurantData.BonusBudget,
                               IdAddress = newAddressQuery.Entity.IdAddress,
                           }
                        );
                    await _context.SaveChangesAsync();

                    var getChefQuery = await _context.Employee
                        .Where(e => e.IsOwner.ToLower() == "y" || e.IsOwner.ToLower() == "t")
                        .FirstAsync();

                    //adding owner to restaurant
                    var newEmployeeHiredQuery = _context.EmployeeRestaurant.Add
                        (
                            new EmployeeRestaurant
                            {
                                IdEmployee = getChefQuery.IdEmployee,
                                IdRestaurant = newRestaurantQuery.Entity.IdRestaurant,
                                IdType = ownerTypeId
                            }
                        );
                    await _context.SaveChangesAsync();

                    var getUserQuery = await _context.User
                        .Where(u => u.IdEmployee == getChefQuery.IdEmployee)
                        .FirstOrDefaultAsync();
                    if (getUserQuery != null)
                    {
                        IEnumerable<int> employeeRoles = _context.EmployeeRestaurant
                            .Where(eir => eir.IdEmployee == getChefQuery.IdEmployee)
                            .Select(eir => eir.IdType);
                        getUserQuery.UserRole = new MapUserRolesUtility(_config).GetUserRoleBasedOnEmployeeTypesId(employeeRoles);
                    }
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

        public async Task<bool> AddNewDishToRestaurantsAsync(PostDishDTO newDishData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var newDishQuery = _context.Dish.Add
                        (
                            new Dish
                            {
                                Name = newDishData.Name,
                                Price = newDishData.Price
                            }
                        );
                    await _context.SaveChangesAsync();

                    foreach (int idRestaurant in newDishData.IdRestaurants)
                    {
                        var newDishInRestaurantQuery = _context.RestaurantDish.Add
                            (
                                    new RestaurantDish
                                    {
                                        IdDish = newDishQuery.Entity.IdDish,
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

        public async Task<bool> AddNewEmployeeToRestaurantAsync(int empId, int typeId, int restaurantId, bool isSupervisorInRestaurant)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var newEmployeeHiredQuery = _context.EmployeeRestaurant.Add
                        (
                            new EmployeeRestaurant
                            {
                                IdEmployee = empId,
                                IdRestaurant = restaurantId,
                                IdType = typeId
                            }
                        );
                    await _context.SaveChangesAsync();

                    var userQuery = await _context.User
                        .Where(u => u.IdEmployee == empId)
                        .FirstOrDefaultAsync();
                    if (userQuery != null)
                    {
                        IEnumerable<int> employeeRoles = _context.EmployeeRestaurant
                            .Where(eir => eir.IdEmployee == empId)
                            .Select(eir => eir.IdType);
                        userQuery.UserRole = new MapUserRolesUtility(_config).GetUserRoleBasedOnEmployeeTypesId(employeeRoles);
                    }
                    await _context.SaveChangesAsync();

                    if (isSupervisorInRestaurant)
                    {
                        //In 100% emp exist in db because inside RestaurantController has already checked does emp exist or not.
                        var getEmpQuery = await _context.Employee
                            .Where(e => e.IdEmployee == empId)
                            .FirstAsync();

                        getEmpQuery.FirstPromotionChefDate = DateTime.Now.Date;

                        await _context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine(ex.ToString());
                    return false;
                }
                await transaction.CommitAsync();
                return true;
            }
        }

        public async Task<bool> UpdateRestaurantDataAsync(int restaurantId, PutRestaurantDTO restaurantData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var getRestaurantQuery = await
                        _context.Restaurant
                        .Where(r => r.IdRestaurant == restaurantId)
                        .FirstAsync();

                    getRestaurantQuery.Name = restaurantData.Name;
                    getRestaurantQuery.RestaurantStatus = restaurantData.Status;
                    getRestaurantQuery.BonusBudget = restaurantData.BonusBudget;

                    await _context.SaveChangesAsync();

                    var getRestaurantAddressQuery =
                        await _context.Address
                        .Where(a => a.IdAddress == getRestaurantQuery.IdAddress)
                        .FirstAsync();

                    getRestaurantAddressQuery.City = restaurantData.Address.City;
                    getRestaurantAddressQuery.Street = restaurantData.Address.Street;
                    getRestaurantAddressQuery.BuildingNumber = restaurantData.Address.BuildingNumber;
                    getRestaurantAddressQuery.LocalNumber = restaurantData.Address.LocalNumber;

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

        public async Task<bool> UpdateDishDataAsync(int dishId, PutDishDTO dishData)
        {
            try
            {
                var getDishQuery = await
                    (_context.Dish
                    .Where(d => d.IdDish == dishId)
                    .FirstAsync());

                getDishQuery.Name = dishData.Name;
                getDishQuery.Price = dishData.Price;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        public async Task<bool> UpdateEmployeeTypeAsync(int empId, int typeId, int restaurantId, bool isSupervisorInRestaurant)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var getEmpTypeInRestaurantQuery = await
                        (_context.EmployeeRestaurant
                        .Where(eir => eir.IdEmployee == empId && eir.IdRestaurant == restaurantId)
                        .FirstAsync());

                    getEmpTypeInRestaurantQuery.IdType = typeId;

                    await _context.SaveChangesAsync();

                    var getUserQuery = await _context.User
                        .Where(u => u.IdEmployee == empId)
                        .FirstOrDefaultAsync();
                    if (getUserQuery != null)
                    {
                        IEnumerable<int> employeeRoles = _context.EmployeeRestaurant
                            .Where(eir => eir.IdEmployee == empId)
                            .Select(eir => eir.IdType);
                        getUserQuery.UserRole = new MapUserRolesUtility(_config).GetUserRoleBasedOnEmployeeTypesId(employeeRoles);
                    }
                    await _context.SaveChangesAsync();

                    if (isSupervisorInRestaurant)
                    {
                        //In 100% emp exist in db because inside RestaurantController has already checked does emp exist or not.
                        var getEmpQuery = await _context.Employee
                            .Where(e => e.IdEmployee == empId)
                            .FirstAsync();

                        getEmpQuery.FirstPromotionChefDate = DateTime.Now.Date;

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
                var getDishInRestaurantQuery = await
                    (_context.RestaurantDish
                    .Where(rd => rd.IdRestaurant == restaurantId && rd.IdDish == dishId)
                    .FirstAsync()
                    );

                _context.Remove(getDishInRestaurantQuery);

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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var getEmployeeInRestaurantQuery = await
                        (_context.EmployeeRestaurant
                        .Where(eir => eir.IdRestaurant == restaurantId && eir.IdEmployee == empId)
                        .FirstAsync());

                    _context.Remove(getEmployeeInRestaurantQuery);
                    await _context.SaveChangesAsync();

                    var getUserQuery = await _context.User
                        .Where(u => u.IdEmployee == empId)
                        .FirstOrDefaultAsync();
                    if (getUserQuery != null)
                    {
                        IEnumerable<int> employeeRoles = _context.EmployeeRestaurant
                            .Where(eir => eir.IdEmployee == empId)
                            .Select(eir => eir.IdType);
                        getUserQuery.UserRole = new MapUserRolesUtility(_config).GetUserRoleBasedOnEmployeeTypesId(employeeRoles);
                    }
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
    }

}
