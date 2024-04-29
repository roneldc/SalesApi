using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalesApi.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Column(TypeName = "date")]
        public DateOnly OrderDate { get; set; }
        [Column(TypeName = "time")]
        public TimeOnly OrderTime { get; set; }
    }
}