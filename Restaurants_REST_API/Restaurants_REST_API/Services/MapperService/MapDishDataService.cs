using Restaurants_REST_API.DTOs.GetDTO;
using Restaurants_REST_API.DTOs.PutDTO;
using Restaurants_REST_API.Models.Database;

namespace Restaurants_REST_API.Services.MapperService
{
    public class MapDishDataService
    {
        private readonly GetDishDTO _dishDetailsDatabase;
        private readonly PutDishDTO _newDishData;
        private readonly Dish dishUpdatedData;

        public MapDishDataService(GetDishDTO dishDetailsDatabase, PutDishDTO newDishData)
        {
            _dishDetailsDatabase = dishDetailsDatabase;
            _newDishData = newDishData;
            dishUpdatedData = new Dish();
        }

        private void UpdatedDishData()
        {
            string oldNameData = _dishDetailsDatabase.Name;
            string newNameData = _newDishData.Name;

            decimal oldPriceData = _dishDetailsDatabase.Price;
            decimal newPriceData = _newDishData.Price;

            //setting name
            if (oldNameData.Equals(newNameData))
            {
                dishUpdatedData.Name = oldNameData;
            }
            else
            {
                dishUpdatedData.Name = newNameData;
            }

            //setting price
            if (oldPriceData == newPriceData)
            {
                dishUpdatedData.Price = oldPriceData;
            }
            else
            {
                dishUpdatedData.Price = newPriceData;
            }
        }

        public Dish GetDishUpdatedData()
        {
            UpdatedDishData();

            return dishUpdatedData;
        }


    }
}
