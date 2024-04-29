using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesApi.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductCode { get; set; } = string.Empty;
        [Required]
        [StringLength(20)]
        public string ProductTypeCode { get; set; } = string.Empty;
        [Required]
        [StringLength(10)]
        public string Size { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}