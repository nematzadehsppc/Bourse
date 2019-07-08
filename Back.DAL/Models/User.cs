using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back.DAL.Models
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        [StringLength(255)]
        public string UserName { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        [StringLength(255)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public int AccessLevel { get; set; }
        
        [Column(TypeName = "VARCHAR(64)")]
        [StringLength(64)]
        public string CheckSum { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        [StringLength(12)]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        
        [DataType(DataType.EmailAddress)]
        [Column(TypeName = "NVARCHAR(255)")]
        [StringLength(255)]
        public string Email { get; set; }

        [Column(TypeName = "DateTime")]
        [DataType(DataType.DateTime)]
        public DateTime? BirthDate { get; set; }

    }
}
