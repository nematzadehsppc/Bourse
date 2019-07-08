using System;
using System.Collections.Generic;
using System.Text;

namespace Back.DAL.Models
{
    /// <summary>
    /// نتیجه تابع تأیید هویت
    /// </summary>
    public enum AuthenticationResultCode
    {
        /// <summary>
        /// نام کاربری یا کلمه عبور اشتباه است
        /// </summary>
        AuthenticationFailure   =   0, 
        /// <summary>
        /// ورود موفق
        /// </summary>
        AuthenticationSuccess   =   1,
        /// <summary>
        /// کاربر دسترسی سیستم مورد نظر را ندارد
        /// </summary>
        InvalidServiceAccessType=   2,
        /// <summary>
        /// اطلاعات ارسالی به متد ناقص است
        /// </summary>
        InvalidInputParams      =   3,
        /// <summary>
        /// خطای اتصال به پایگاه داده ها
        /// </summary>
        DbConnectionError       =   4,        
        /// <summary>
        /// خطای پیش‌بینی نشده
        /// </summary>
        UnknownServerError      =   5,
        /// <summary>
        /// خطای عدم اتصال کلاینت
        /// </summary>
        ClientConnectivityError =   10

    }
}
