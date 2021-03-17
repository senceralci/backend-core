using System.Security.Cryptography;
using System.Text;

namespace Core.Utilities.Security.Hashing
{
    public class HashingHelper
    {
        public static void CreateHash(string value, out byte[] hashedValue, out byte[] saltKey)
        {
            using (var hmac = new HMACSHA512())
            {
                saltKey = hmac.Key;
                hashedValue = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            }
        }

        public static bool VerifyHash(string value, byte[] hashedValue, byte[] saltKey)
        {
            using (var hmac = new HMACSHA512(saltKey))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
                for (var i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != hashedValue[i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}