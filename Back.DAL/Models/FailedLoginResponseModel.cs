using Back.DAL.Models;

namespace Back.DAL.Models
{
    /// <summary>
    /// خروجی متد ورود به سیستم در حالت ناموفق
    /// </summary>
    public class FailedLoginResponseModel
    {
        /// <summary>
        /// نام کاربری یا کلمه عبور اشتباه است - AuthenticationFailure   =   0, 
        ///ورود موفق AuthenticationSuccess   =   1,
        ///کاربر دسترسی سیستم مورد نظر را ندارد InvalidServiceAccessType=   2,
        ///اطلاعات ارسالی به متد ناقص است InvalidInputParams      =   3,
        ///خطای اتصال به پایگاه داده ها DbConnectionError       =   4,        
        ///خطای پیش‌بینی نشده UnknownServerError      =   5,
        ///خطای عدم اتصال کلاینت - این کد خطا را خود کلاینت می‌تواند تولید کند و از سرور بر نمی‌گرددClientConnectivityError =   10
        /// </summary>
        public AuthenticationResultCode code { get; set; }

        /// <summary>
        /// معادل رشته‌ای خطا مطابق زبان سیستم (فارسی / انگلیسی)
        /// </summary>
        public string authenticationResult { get; set; }

        /// <summary>
        /// در مواردی مانند خطای ناشناخته توضیحات تکمیلی شامل رشته استثنا در آن برگردانده می‌شود
        /// </summary>
        public string additionalInformation { get; set; }
    }
}
