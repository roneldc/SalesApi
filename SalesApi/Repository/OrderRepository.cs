using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using SalesApi.Data;
using SalesApi.Dtos;
using SalesApi.Helpers;
using SalesApi.Interface;
using SalesApi.Mappers;
using SalesApi.Models;

namespace SalesApi.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SalesDbContext _context;
        public OrderRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> GetAllAsync(OrderQueryObject query)
        {
            var products = _context.Product.AsQueryable();
            var productTypes = _context.ProductType.AsQueryable();
            var orders = _context.Orders.AsQueryable();
            var orderDetails = _context.OrderDetails.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.ProductCode))
            {
                products = products.Where(p => p.ProductCode.Contains(query.ProductCode));
            }

            if (!string.IsNullOrWhiteSpace(query.ProductTypeCode))
            {
                products = products.Where(p => p.ProductTypeCode.Contains(query.ProductTypeCode));
            }

            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                productTypes = productTypes.Where(p => p.Name.Contains(query.Name));
            }

            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                productTypes = productTypes.Where(p => p.Category.Contains(query.Category));
            }

            if (!string.IsNullOrWhiteSpace(query.Ingredients))
            {
                productTypes = productTypes.Where(p => p.Ingredients.Contains(query.Ingredients));
            }

            var result = from product in products
                         join productType in productTypes on product.ProductTypeCode equals productType.ProductTypeCode
                         join orderDetail in orderDetails on product.ProductCode equals orderDetail.ProductCode
                         join order in orders on orderDetail.OrderId equals order.Id
                         select new OrderDto()
                         {
                             Id = product.Id,
                             ProductCode = product.ProductCode,
                             ProductTypeCode = product.ProductTypeCode,
                             Name = productType.Name,
                             Category = productType.Category,
                             Ingredients = productType.Ingredients,
                             Size = product.Size,
                             Price = product.Price,
                             Quantity = orderDetail.Quantity,
                             OrderDate = order.OrderDate,
                             OrderTime = order.OrderTime
                         };

            return await result.Take(query.PageSize).ToListAsync();
        }

        public async Task<OrderDto?> GetByProductCodeAsync(string productCode)
        {
            var products = await _context.Product.ToListAsync();
            var productTypes = await _context.ProductType.ToListAsync();
            var orders = await _context.Orders.ToListAsync();
            var orderDetails = await _context.OrderDetails.ToListAsync();

            var result = from product in products
                         join productType in productTypes on product.ProductTypeCode equals productType.ProductTypeCode
                         join orderDetail in orderDetails on product.ProductCode equals orderDetail.ProductCode
                         join order in orders on orderDetail.OrderId equals order.Id
                         select new OrderDto()
                         {
                             Id = product.Id,
                             ProductCode = product.ProductCode,
                             ProductTypeCode = product.ProductTypeCode,
                             Name = productType.Name,
                             Category = productType.Category,
                             Ingredients = productType.Ingredients,
                             Size = product.Size,
                             Price = product.Price,
                             Quantity = orderDetail.Quantity,
                             OrderDate = order.OrderDate,
                             OrderTime = order.OrderTime
                         };

            return result.FirstOrDefault(r => r.ProductCode == productCode);
        }

        public async Task UploadOrderAsync(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                csvReader.Context.Configuration.HeaderValidated = null;
                csvReader.Context.RegisterClassMap<OrderMap>();
                var records = csvReader.GetRecords<Order>();

                await _context.Orders.AddRangeAsync(records);
                await _context.SaveChangesAsync();
            }
        }
    }
}