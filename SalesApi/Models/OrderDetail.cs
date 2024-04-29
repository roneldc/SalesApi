using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesApi.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderDetailsId { get; set; }
        [Required]
        [Range(1, 10000)]
        public int Quantity { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        [StringLength(50)]
        public string ProductCode { get; set; } = string.Empty;

    }
}