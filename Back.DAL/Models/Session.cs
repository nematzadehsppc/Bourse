using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back.DAL.Models
{
    public class Session
    {
        public int Id { get; set; }

        /// <summary>
        /// شناسه کاربر
        /// </summary>
        public int UserId { get; set; }
        
        public byte[] UserSession { get; set; }

        /// <summary>
        /// شناسه سشن
        /// </summary>
        public Guid SessionId { get; set; }

        /// <summary>
        /// اندازهٔ فایل
        /// </summary>
        public long FileSize
        {
            get;
            set;
        }

        public string MCheckSum
        {
            get;
            set;
        }
    }
}
