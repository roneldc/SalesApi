using CsvHelper.Configuration;
using SalesApi.Models;

namespace SalesApi.Mappers
{
    public class OrderDetailMap : ClassMap<OrderDetail>
    {
        public OrderDetailMap()
        {
            Map(m => m.OrderDetailsId).Index(0);
            Map(m => m.OrderId).Index(1);
            Map(m => m.ProductCode).Index(2);
            Map(m => m.Quantity).Index(3);
        }
    }
}