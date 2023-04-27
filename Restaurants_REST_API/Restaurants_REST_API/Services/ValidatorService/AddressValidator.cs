using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PostOrPutDTO;

namespace Restaurants_REST_API.Services.ValidatorService
{
    public class AddressValidator
    {
        public static bool isEmptyAddressOf(AddressDTO newAddress)
        {
            if (newAddress.City == null || newAddress.Street == null || newAddress.BuildingNumber == null)
            {
                return true;
            }

            if (newAddress.City.Replace("\\s", "") == "")
            {
                return true;
            }

            if (newAddress.Street.Replace("\\s", "") == "")
            {
                return true;
            }

            if (newAddress.BuildingNumber.Replace("\\s", "") == "")
            {
                return true;
            }

            return false;
        }
    }
}
