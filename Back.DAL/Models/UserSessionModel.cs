using System;
using System.Collections.Generic;
using System.Text;

namespace Back.DAL.Models
{
    /// <summary>
    /// مدل به روز آوری توکن
    /// </summary>
    public class UserSessionModel
    {
        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// شناسه جلسه کاربر
        /// </summary>
        public Guid SessionId { get; set; }
    }
}
