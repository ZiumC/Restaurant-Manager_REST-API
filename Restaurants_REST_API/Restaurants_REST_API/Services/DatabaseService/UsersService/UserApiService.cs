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

        public Task<bool> RegisterNewClientAsync(User newClient)
        {
            throw new NotImplementedException();
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
            return await _context.User.Where(u => u.Email == email).FirstOrDefaultAsync();
        }
    }
}
