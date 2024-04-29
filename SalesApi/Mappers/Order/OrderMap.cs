using CsvHelper.Configuration;
using SalesApi.Models;

namespace SalesApi.Mappers
{
    public class OrderMap : ClassMap<Order>
    {
        public OrderMap()
        {
            Map(m => m.OrderId).Index(0);
            Map(m => m.OrderDate).Index(1);
            Map(m => m.OrderTime).Index(2);
        }
    }
}