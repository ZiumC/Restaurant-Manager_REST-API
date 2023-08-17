namespace Restaurants_REST_API.Services.ValidatorService
{
    public class GeneralValidator
    {
        public static bool isNumberGtZero(int? id)
        {
            if (id == null)
            {
                return false;
            }

            return id > 0;
        }

        public static bool isEmptyNameOf(string? fieldToCheck)
        {
            if (fieldToCheck == null)
            {
                return true;
            }

            return fieldToCheck.Replace("\\s", "") == "";
        }

        public static bool isCorrectBonus(decimal current, decimal minimum)
        {
            return current >= minimum;
        }
    }
}
