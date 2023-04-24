using Restaurants_REST_API.DTOs;
using System.Text.RegularExpressions;

namespace Restaurants_REST_API.Services.ValidatorService
{
    public class RestaurantValidator
    {
        public static bool isEmptyNameOf(string field)
        {
            if (field == null)
            {
                return true;
            }

            if (field.Replace("\\s", "").Equals(""))
            {
                return true;
            }

            return false;
        }

        public static bool isRestaurantExistIn(IEnumerable<RestaurantDTO>? allRestaurants, RestaurantDTO newRestaurant)
        {
            if (allRestaurants == null)
            {
                return false;
            }

            List<bool> newRestaurantEquals = new List<bool>();

            foreach (RestaurantDTO restaurant in allRestaurants)
            {
                bool restaurantExist = true;
                if (!restaurant.Name.Equals(newRestaurant.Name))
                {
                    restaurantExist = false;
                }

                if (!restaurant.Status.Equals(newRestaurant.Status))
                {
                    restaurantExist = false;
                }

                if (!restaurant.Address.City.Equals(newRestaurant.Address.City))
                {
                    restaurantExist = false;
                }

                if (!restaurant.Address.Street.Equals(newRestaurant.Address.Street))
                {
                    restaurantExist = false;
                }

                if (!restaurant.Address.BuildingNumber.Equals(newRestaurant.Address.BuildingNumber))
                {
                    restaurantExist = false;
                }

                newRestaurantEquals.Add(restaurantExist);
            }

            return newRestaurantEquals.Contains(true);
        }
    }
}
