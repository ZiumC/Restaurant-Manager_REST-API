using Microsoft.EntityFrameworkCore;
using Restaurants_REST_API.DbContexts;
using Restaurants_REST_API.Models.Database;
using Restaurants_REST_API.Models.DatabaseModel;

namespace Restaurants_REST_API.Services.DatabaseService.UsersService
{
    public class UserApiService : IUserApiService
    {
        private readonly MainDbContext _context;
        public UserApiService(MainDbContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterNewClientAsync(User registerClient)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var addQuery = _context.Client.Add(new Client
                    {
                        Name = registerClient.Login,
                        IsBusinessman = "N"
                    });
                    await _context.SaveChangesAsync();

                    registerClient.IdClient = addQuery.Entity.IdClient;
                    registerClient.Client = addQuery.Entity;

                    _context.User.Add(registerClient);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    await transaction.RollbackAsync();
                    return false;
                }
            }
        }

        public async Task<bool> RegisterNewEmployeeAsync(User registerEmployee, string pesel)
        {
            try
            {
                Employee? getEmployeeQuery = await _context.Employee
                    .Where(e => e.PESEL == pesel)
                    .FirstOrDefaultAsync();

                if (getEmployeeQuery == null)
                {
                    return false;
                }

                registerEmployee.IdEmployee = getEmployeeQuery.IdEmployee;
                registerEmployee.Employee = getEmployeeQuery;

                _context.Add(registerEmployee);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public async Task<User?> GetUserDataBy(string email)
        {
            return await _context.User.Where(u => u.Email.ToLower() == email.ToLower()).FirstOrDefaultAsync();
        }
    }
}
