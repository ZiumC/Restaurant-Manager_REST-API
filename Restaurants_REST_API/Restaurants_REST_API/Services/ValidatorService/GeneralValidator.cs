namespace Restaurants_REST_API.Services.ValidatorService
{
    public class GeneralValidator
    {
        public static bool isIntNumberGtZero(int? val)
        {
            if (val == null)
            {
                return false;
            }

            return val > 0;
        }

        public static bool isDecimalNumberGtZero(decimal? val) 
        {
            if (val == null)
            {
                return false;
            }

            return val > new decimal(0.0);
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
