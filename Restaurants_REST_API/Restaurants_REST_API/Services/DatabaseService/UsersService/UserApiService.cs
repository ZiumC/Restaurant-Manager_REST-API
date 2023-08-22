using Restaurants_REST_API.DTOs.PostDTO;

namespace Restaurants_REST_API.Services.DatabaseService.UsersService
{
    public class UserApiService : IUserApiService
    {
        public Task<bool> RegisterNewClientAsync(PostUserDTO newUser)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RegisterNewEmployeeAsync(PostUserDTO newUser)
        {
            throw new NotImplementedException();
        }
    }
}
