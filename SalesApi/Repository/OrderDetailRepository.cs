using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using SalesApi.Data;
using SalesApi.Interface;
using SalesApi.Mappers;
using SalesApi.Models;

namespace SalesApi.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly SalesDbContext _context;
        public OrderDetailRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task UploadOrderDetailsAsync(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                csvReader.Context.Configuration.HeaderValidated = null;
                csvReader.Context.RegisterClassMap<OrderDetailMap>();
                var records = csvReader.GetRecords<OrderDetail>();

                await _context.OrderDetails.AddRangeAsync(records);
                await _context.SaveChangesAsync();
            }
        }
    }
}