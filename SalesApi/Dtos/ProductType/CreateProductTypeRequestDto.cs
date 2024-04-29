using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalesApi.Dtos
{
    public class CreateProductTypeRequestDto
    {
        [Required]
        [StringLength(20, ErrorMessage = "Product Type code cannot be over 20 characters.")]
        public string ProductTypeCode { get; set; } = string.Empty;
        [Required]
        [StringLength(100, ErrorMessage = "Product name cannot be over 100 characters.")]
        public string Name { get; set; } = string.Empty;
        [Required]
        [StringLength(50, ErrorMessage = "Category cannot be over 50 characters.")]
        public string Category { get; set; } = string.Empty;
        [Required]
        [StringLength(250, ErrorMessage = "Ingredients cannot be over 250 characters.")]
        public string Ingredients { get; set; } = string.Empty;
    }
}