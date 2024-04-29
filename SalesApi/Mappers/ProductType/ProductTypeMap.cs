using CsvHelper.Configuration;
using SalesApi.Models;

namespace SalesApi.Mappers
{
    public class ProductTypeMap : ClassMap<ProductType>
    {
        public ProductTypeMap()
        {
            Map(m => m.ProductTypeCode).Index(0);
            Map(m => m.Name).Index(1);
            Map(m => m.Category).Index(2);
            Map(m => m.Ingredients).Index(3);
        }
    }
}