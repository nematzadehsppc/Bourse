using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back.DAL.Models
{

    public class ParamType
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        [StringLength(255)]
        public string Name { get; set; }
        
    }
}