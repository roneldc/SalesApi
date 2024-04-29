using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesApi.Dtos
{
    public class ProductTypeDto
    {
        public int Id { get; set; }
        public string ProductTypeCode { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Ingredients { get; set; } = string.Empty;
    }
}