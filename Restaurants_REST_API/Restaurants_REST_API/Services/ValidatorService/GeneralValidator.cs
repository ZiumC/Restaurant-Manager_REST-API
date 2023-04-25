namespace Restaurants_REST_API.Services.ValidatorService
{
    public class GeneralValidator
    {
        public static bool isCorrectId(int id)
        {
            return id > 0;
        }
    }
}
