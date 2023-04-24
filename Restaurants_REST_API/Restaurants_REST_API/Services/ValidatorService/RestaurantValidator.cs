using Restaurants_REST_API.DTOs;

namespace Restaurants_REST_API.Services.ValidatorService
{
    public class RestaurantValidator
    {
        public static bool isEmptyNameOf(RestaurantDTO restaurantToCheck)
        {
            if (restaurantToCheck.Name.Replace("\\s", "").Equals(""))
            {
                return true;
            }

            return false;
        }
    }
}
