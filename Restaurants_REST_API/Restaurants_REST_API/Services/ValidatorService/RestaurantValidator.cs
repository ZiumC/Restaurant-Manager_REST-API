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
    }
}
