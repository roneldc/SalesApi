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
    public class ProductRepository : IProductRepository
    {
        private readonly SalesDbContext _context;

        public ProductRepository(SalesDbContext context)
        {
            _context = context;
        }

        public async Task<Product> CreateProductAsync(Product productModel)
        {
            await _context.Product.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        public async Task<List<ProductDto>> GetAllAsync(ProductQueryObject query)
        {
            var products = _context.Product.AsQueryable();
            var productTypes = _context.ProductType.AsQueryable();

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
                         select new ProductDto()
                         {
                             Id = product.Id,
                             ProductCode = product.ProductCode,
                             ProductTypeCode = product.ProductTypeCode,
                             Name = productType.Name,
                             Category = productType.Category,
                             Ingredients = productType.Ingredients,
                             Size = product.Size,
                             Price = product.Price,
                             ProducTypeId = productType.Id
                         };

            return await result.ToListAsync();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var products = await _context.Product.ToListAsync();
            var productTypes = await _context.ProductType.ToListAsync();

            var result = from product in products
                         join productType in productTypes on product.ProductTypeCode equals productType.ProductTypeCode
                         select new ProductDto()
                         {
                             Id = product.Id,
                             ProductCode = product.ProductCode,
                             ProductTypeCode = product.ProductTypeCode,
                             Name = productType.Name,
                             Category = productType.Category,
                             Ingredients = productType.Ingredients,
                             Size = product.Size,
                             Price = product.Price,
                             ProducTypeId = productType.Id
                         };

            return result.FirstOrDefault(r => r.Id == id);
        }

        public async Task UploadProductAsync(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                csvReader.Context.Configuration.HeaderValidated = null;
                csvReader.Context.RegisterClassMap<ProductMap>();
                var records = csvReader.GetRecords<Product>();

                await _context.Product.AddRangeAsync(records);
                await _context.SaveChangesAsync();
            }
        }
    }
}