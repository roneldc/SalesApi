namespace SalesApi.Helpers
{
    public class ProductQueryObject
    {
        public string? ProductCode { get; set; } = null;
        public string? ProductTypeCode { get; set; } = null;
        public string? Name { get; set; } = null;
        public string? Category { get; set; } = null;
        public string? Ingredients { get; set; } = null;
    }
}