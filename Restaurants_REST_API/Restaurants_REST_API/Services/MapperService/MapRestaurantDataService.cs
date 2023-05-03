using Restaurants_REST_API.DTOs.GetDTOs;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.MapperService
{
    public class MapRestaurantDataService
    {
        private readonly GetRestaurantDTO _restaurantDetailsDatabase;
        private readonly PutRestaurantDTO _newRestaurantData;
        private readonly Restaurant restaurantUpdatedData;

        public MapRestaurantDataService(GetRestaurantDTO restaurantDetailsDatabase, PutRestaurantDTO newRestaurantData)
        {
            _restaurantDetailsDatabase = restaurantDetailsDatabase;
            _newRestaurantData = newRestaurantData;
            restaurantUpdatedData = new Restaurant();
            restaurantUpdatedData.Address = new Address();
        }

        private void UpdatedRestaurantData()
        {
            string oldNameData = _restaurantDetailsDatabase.Name;
            string newNameData = _newRestaurantData.Name;

            string oldStatusData = _restaurantDetailsDatabase.Status;
            string newStatusData = _newRestaurantData.Status;

            decimal? oldBonusBudgetData = _restaurantDetailsDatabase.BonusBudget;
            decimal? newBonusBudgetData = _newRestaurantData.BonusBudget;

            //setting name
            if (oldNameData.Equals(newNameData))
            {
                restaurantUpdatedData.Name = oldNameData;
            }
            else
            {
                restaurantUpdatedData.Name = newNameData;
            }

            //setting status
            if (oldStatusData.Equals(newStatusData))
            {
                restaurantUpdatedData.RestaurantStatus = oldStatusData;
            }
            else
            {
                restaurantUpdatedData.RestaurantStatus = newStatusData;
            }

            //setting bonus budget
            if (newBonusBudgetData != null && oldBonusBudgetData != null)
            {
                if (oldBonusBudgetData == newBonusBudgetData)
                {
                    restaurantUpdatedData.BonusBudget = oldBonusBudgetData;
                }
                else
                {
                    restaurantUpdatedData.BonusBudget = newBonusBudgetData;
                }
            }
            else
            {
                restaurantUpdatedData.BonusBudget = newBonusBudgetData;
            }
        }

        private void UpdateRestaurantAddress()
        {
            string oldCityData = _restaurantDetailsDatabase.Address.City;
            string newCityData = _newRestaurantData.Address.City;

            string oldStreetData = _restaurantDetailsDatabase.Address.Street;
            string newStreetData = _newRestaurantData.Address.Street;

            string oldBuildingNumberData = _restaurantDetailsDatabase.Address.BuildingNumber;
            string newBuildingNumberData = _newRestaurantData.Address.BuildingNumber;

            string? oldLocalNumber = _restaurantDetailsDatabase.Address.LocalNumber;
            string? newLocalNumber = _newRestaurantData.Address.LocalNumber;

            //setting city address 
            if (newCityData.Equals(oldCityData))
            {
                restaurantUpdatedData.Address.City = oldCityData;
            }
            else
            {
                restaurantUpdatedData.Address.City = newCityData;
            }

            //setting street address 
            if (newStreetData.Equals(oldStreetData))
            {
                restaurantUpdatedData.Address.Street = oldStreetData;
            }
            else
            {
                restaurantUpdatedData.Address.Street = newStreetData;
            }

            //setting building number address 
            if (newBuildingNumberData.Equals(oldBuildingNumberData))
            {
                restaurantUpdatedData.Address.BuildingNumber = oldBuildingNumberData;
            }
            else
            {
                restaurantUpdatedData.Address.BuildingNumber = newBuildingNumberData;
            }

            //setting local number
            if (newLocalNumber != null && oldLocalNumber != null)
            {
                if (newLocalNumber.Equals(oldLocalNumber))
                {
                    restaurantUpdatedData.Address.LocalNumber = oldLocalNumber;
                }
                else
                {
                    restaurantUpdatedData.Address.LocalNumber = newLocalNumber;
                }
            }
            else
            {
                restaurantUpdatedData.Address.LocalNumber = newLocalNumber;
            }
        }

        public Restaurant GetRestaurantUpdatedData()
        {
            UpdatedRestaurantData();
            UpdateRestaurantAddress();

            return restaurantUpdatedData;
        }
    }
}
