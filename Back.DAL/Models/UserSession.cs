using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back.DAL.Models
{
    public class UserSession
    {
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// شناسه سشن
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// آی پی آدرس یا ...
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// اطلاعات برنامه کلاینت
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// تاریخ و ساعت ورود به سیستم
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// آخرین زمان به روزآوری
        /// </summary>
        public DateTime LastRenewal { get; set; }

        /// <summary>
        /// اعتبار
        /// </summary>
        public DateTime ValidUntil { get; set; }

        /// <summary>
        /// چک سام اطلاعات کاربر در زمان ورود
        /// </summary>
        public string UserMCheckSum { get; set; }

        /// <summary>
        /// توکن کاربر
        /// </summary>
        public string Token { get; set; }


        /// <summary>
        /// سرویس مورد نیاز
        /// </summary>
        public ServiceAccesType ServiceAccessType { get; set; }

        /// <summary>
        /// زبان
        /// </summary>
        public string Language { get; set; }
    }
}
