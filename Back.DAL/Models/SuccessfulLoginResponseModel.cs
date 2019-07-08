using System;

namespace Back.DAL.Models
{
    /// <summary>
    /// خروجی متد ورود موفق به سیستم
    /// </summary>
    public class SuccessfulLoginResponseModel
    {
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// شناسه جلسه ورود کاربر - یکتا برای کاربر
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// تاریخ و ساعت ورود به سیستم
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// توکن امنیتی جلسه
        /// </summary>
        public string Token { get; set; }
    }
}
