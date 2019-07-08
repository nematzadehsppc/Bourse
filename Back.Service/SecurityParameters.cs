
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
    }
}
