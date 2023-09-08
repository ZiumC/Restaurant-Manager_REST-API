using Restaurants_REST_API.Models.DatabaseModel;

namespace Restaurants_REST_API.Services.DatabaseService.UsersService
{
    public interface IUserApiService
    {
        public Task<bool> RegisterNewClientAsync(User registerClient);
        public Task<bool> RegisterNewEmployeeAsync(User registerEmployee);
        public Task<User?> GetUserDataByEmpIdAsync(int empId);
        public Task<User?> GetUserDataByLoginOrEmailAsync(string loginOrEmil);
        public Task<User?> GetUserDataByRefreshTokenAsync(string refreshToken);
        public Task<bool> UpdateUserDataAsync(User userData);
    }
}
