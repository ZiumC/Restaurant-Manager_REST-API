namespace Restaurants_REST_API.Utils.ValidatorService
{
    public class GeneralValidatorUtility
    {

        /// <summary>
        /// Checks if int value is not null. Then checks if number is gt zero.
        /// </summary>
        /// <param name="val">Int number to check.</param>
        /// <returns>True if number is greater than zero. False otherwise.</returns>
        public static bool isIntNumberGtZero(int? val)
        {
            if (val == null)
            {
                return false;
            }

            return val > 0;
        }

        /// <summary>
        /// Checks if decimal value is not null. Then checks if number is gt zero.
        /// </summary>
        /// <param name="val">Decimal number to check.</param>
        /// <returns>True if number is greater than zero. False otherwise.</returns>
        public static bool isDecimalNumberGtZero(decimal? val)
        {
            if (val == null)
            {
                return false;
            }

            return val > new decimal(0.0);
        }
    }
}
