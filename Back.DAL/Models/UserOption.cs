using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Back.DAL.Models
{
    public class UserOption
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// شناسه کاربر
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string OptionValue { get; set; }
    }
}
