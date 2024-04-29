using CsvHelper.Configuration;
using SalesApi.Models;

namespace SalesApi.Mappers
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Map(m => m.ProductCode).Index(0);
            Map(m => m.ProductTypeCode).Index(1);
            Map(m => m.Size).Index(2);
            Map(m => m.Price).Index(3);
        }
    }
}