using Restaurants_REST_API.Models.DatabaseModel;

namespace Restaurants_REST_API.Services.DatabaseService.UsersService
{
    public interface IUserApiService
    {
        public Task<bool> RegisterNewClientAsync(User newClient);
        public Task<bool> RegisterNewEmployeeAsync(User newEmployee);
    }
}
