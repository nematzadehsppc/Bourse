
using System.Security.Cryptography;
using System.Text;

namespace BourseService
{
    class SecurityParameters
    {
        /// <summary>
        /// رمز استفاده شده جهت تولید توکنهای برنامه
        /// </summary>
        public const string Secret = "d39cde2c04de6d32a51be1b4d4c8060d57ad5ae0";

        /// <summary>
        /// زمان پیش‌فرض اکسپایر توکنها
        /// </summary>
        public const int DefaultTokenExpirationInSeconds = 3600;

        public static string MD5Encryption(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("{0:X2}"));
            }
            return hash.ToString();
        }
    }
}
