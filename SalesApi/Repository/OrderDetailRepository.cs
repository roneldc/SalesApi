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
    /// <summary>
    /// Repository for handling Order Detail related operations.
    /// </summary>
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly SalesDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderDetailRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public OrderDetailRepository(SalesDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Uploads the order detail data from a csv file.
        /// </summary>
        /// <param name="file">The file containing order detail data.</param>
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