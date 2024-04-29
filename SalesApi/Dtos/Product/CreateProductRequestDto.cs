using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SalesApi.Dtos
{
    public class CreateProductRequestDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Product code cannot be over 50 characters.")]
        public string ProductCode { get; set; } = string.Empty;
        [Required]
        [StringLength(20, ErrorMessage = "Product type code cannot be over 20 characters.")]
        public string ProductTypeCode { get; set; } = string.Empty;
        [Required]
        [StringLength(10, ErrorMessage = "Size cannot be over 10 characters.")]
        public string Size { get; set; } = string.Empty;
        [Required]
        [Range(0.00, 1000000000, ErrorMessage = "Invalid price value.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}