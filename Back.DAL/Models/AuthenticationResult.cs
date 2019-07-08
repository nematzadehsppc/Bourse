

namespace Back.DAL.Models
{
    /// <summary>
    /// نتیجه اجرای ورود به سیستم
    /// </summary>
    public class AuthenticationResult
    {

        public AuthenticationResult(AuthenticationResultCode code, string msg = "")
        {
            Code = code;
            AdditionalErrorMessage = msg;
        }

        /// <summary>
        /// کد نتیجه
        /// </summary>
        public AuthenticationResultCode Code { get; set; }

        /// <summary>
        /// خطای تکمیلی
        /// </summary>
        public string AdditionalErrorMessage { get; set; }

    }
}
