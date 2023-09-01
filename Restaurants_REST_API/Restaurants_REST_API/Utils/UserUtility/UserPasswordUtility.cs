using System.Security.Cryptography;
using System.Text;

namespace Restaurants_REST_API.Utils.UserUtility
{
    public class UserPasswordUtility
    {
        private static readonly int _minSaltLength = 1;
        private static readonly int _maxSaltLength = 10;

        private static readonly int _minPepperLength = 1;
        private static readonly int _maxPepperLength = 7;

        /// <summary>
        /// Method generates an unique salt for password.
        /// </summary>
        /// <param name="length">Length of salt.</param>
        /// <param name="saltBase">Chars that will be used to salt.</param>
        /// <returns>String - generated salt to password.</returns>
        /// <exception cref="Exception">Is raised when length of salt is invalid.</exception>
        public static string GetSalt(int length, string saltBase)
        {
            if (length < _minSaltLength || length > _maxSaltLength)
            {
                throw new Exception($"Salt password length should be in range {_minSaltLength} - {_maxSaltLength}");
            }

            string result = "";

            for (int i = 0; i < length; i++)
            {
                Random random = new Random();
                char c = saltBase[random.Next(saltBase.Length)];
                result = result + c;
            }

            return result;
        }

        /// <summary>
        /// Method hashes user password with pepper and salt.
        /// </summary>
        /// <param name="password">User password.</param>
        /// <param name="salt">Generated salt for user password.</param>
        /// <param name="pepper">Server pepper.</param>
        /// <returns>String - hashed password using SHA256 algorithm.</returns>
        /// <exception cref="Exception">Is raised when length of password pepper is invalid.</exception>
        public static string GetHashedPasswordWithSalt(string password, string salt, string pepper)
        {
            if (pepper.Length < _minPepperLength || pepper.Length > _maxPepperLength)
            {
                throw new Exception($"Pepper password length should be in range {_minPepperLength} - {_maxPepperLength}");
            }

            var sha256algorithm = SHA256.Create();
            var hash = new StringBuilder();

            byte[] crypto = sha256algorithm.ComputeHash(Encoding.UTF8.GetBytes(pepper + password + salt));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }

            return hash.ToString();
        }
    }
}
