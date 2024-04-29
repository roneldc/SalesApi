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
    /// <summary>
    /// Repository for handling Product related operations.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly SalesDbContext _context;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>

        public ProductRepository(SalesDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productModel">The product model.</param>
        /// <returns>The created product.</returns>
        public async Task<Product> CreateProductAsync(Product productModel)
        {
            await _context.Product.AddAsync(productModel);
            await _context.SaveChangesAsync();
            return productModel;
        }

        /// <summary>
        /// Gets all products based on the provided query.
        /// </summary>
        /// <param name="query">The query parameters for filtering products.</param>
        /// <returns>A list of products.</returns>
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

        /// <summary>
        /// Gets the product by id.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <returns>The product associated with the id.</returns>
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

        /// <summary>
        /// Uploads the product data from a csv file.
        /// </summary>
        /// <param name="file">The csv file containing product data.</param>
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