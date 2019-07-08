
namespace Back.DAL.Models
{
    /// <summary>
    /// ورودی متد ورود به سیستم
    /// </summary>
    public class LoginViewModel
    {
        /// <summary>
        /// نام کاربری
        /// </summary>
        /// <example>
        /// admin
        /// </example>        
        public string Username { get; set; }

        /// <summary>
        /// کلمه عبور
        /// </summary>
        /// <example>
        /// 123456
        /// </example>
        public string Password { get; set; }

        /// <summary>
        /// سرویس مورد درخواست
        /// </summary>
        /// <example>
        /// 2
        /// </example>
        public ServiceAccesType ServiceAccessType { get; set; }

        /// <summary>
        /// نام برنامه کلاینت - Tadbir Web
        /// </summary>
        /// <example>
        /// Swagger API Client
        /// </example>
        public string ClientAppName { get; set; }

        /// <summary>
        /// زبان ورودی - fa-IR
        /// </summary>
        /// <example>
        /// fa-ir
        /// </example>
        public string Language { get; set; }
    }
}
