using Restaurants_REST_API.Models.DatabaseModel;

namespace Restaurants_REST_API.Services.DatabaseService.UsersService
{
    public interface IUserApiService
    {
        public Task<bool> RegisterNewClientAsync(User registerClient);
        public Task<bool> RegisterNewEmployeeAsync(User registerEmployee);
        public Task<User?> GetUserDataByEmpId(int empId);
        public Task<User?> GetUserDataByEmail(string email);
        public Task<User?> GetUserDataByLoginOrEmail(string loginOrEmil);
        public Task<User?> GetUserDataByRefreshToken(string refreshToken);
        public Task<bool> UpdateUserData(User userData);
    }
}
