using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesApi.Helpers
{
    public class OrderQueryObject
    {
        public string? ProductCode { get; set; } = null;
        public string? ProductTypeCode { get; set; } = null;
        public string? Name { get; set; } = null;
        public string? Category { get; set; } = null;
        public string? Ingredients { get; set; } = null;
        public int PageSize { get; set; } = 30;
    }
}