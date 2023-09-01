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

        public async Task<User?> GetUserDataByEmpId(int empId)
        {
            return await _context.User.Where(u => u.IdEmployee == empId).FirstOrDefaultAsync();
        }

        public async Task<bool> RegisterNewEmployeeAsync(User registerEmployee)
        {
            try
            {
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

        public async Task<User?> GetUserDataByLoginOrEmail(string loginOrEmil)
        {
            return await _context.User
                .Where(u => u.Email == loginOrEmil || u.Login == loginOrEmil)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateUserData(User userData)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var updateUserData = await (_context.User
                        .Where(u => u.IdUser == userData.IdUser)
                        ).FirstAsync();

                    updateUserData.LoginAttempts = userData.LoginAttempts;
                    updateUserData.DateBlockedTo = userData.DateBlockedTo;

                    if (!string.IsNullOrEmpty(userData.RefreshToken))
                    {
                        updateUserData.RefreshToken = userData.RefreshToken;
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

        public async Task<User?> GetUserDataByRefreshToken(string refreshToken)
        {
            return await _context.User
                .Where(u => u.RefreshToken == refreshToken
                ).FirstOrDefaultAsync();
        }
    }
}
