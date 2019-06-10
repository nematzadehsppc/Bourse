using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back.DAL.Models
{
    public class ParamValue
    {
        public int Id { get; set; }
        
        [Required]
        [DataType(DataType.DateTime)]
        [Column(TypeName = "DateTime")]
        public DateTime TradingDate { get; set; }
        
        [Required]
        [Column(TypeName = "DECIMAL(18, 2)")]
        public double Value { get; set; }

        [Required]
        [Column("SymbolId")]
        public int SymbolId { get; set; }

        [Required]
        [Column("ItemId")]
        public int ItemId { get; set; }

        [ForeignKey("SymbolId")]
        public virtual Symbol Symbol { get; set; }

        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
    }
}