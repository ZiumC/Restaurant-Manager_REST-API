using Restaurants_REST_API.DTOs.PostDTO;

namespace Restaurants_REST_API.Services.DatabaseService.UsersService
{
    public interface IUserApiService
    {
        public Task<bool> RegisterNewClientAsync(PostUserDTO newUser);
        public Task<bool> RegisterNewEmployeeAsync(PostUserDTO newUser);
    }
}
