using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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
    public class ProductTypeRepository : IProductTypeRepository
    {
        private readonly SalesDbContext _context;
        public ProductTypeRepository(SalesDbContext context)
        {
            _context = context;
        }
        public async Task<ProductType> CreateProductTypeAsync(ProductType productType)
        {
            await _context.ProductType.AddAsync(productType);
            await _context.SaveChangesAsync();
            return productType;
        }

        public async Task<ProductType?> DeleteProductTypeAsync(int id)
        {
            var productType = await _context.ProductType.FirstOrDefaultAsync(p => p.Id == id);

            if (productType == null)
            {
                return null;
            }

            _context.ProductType.Remove(productType);
            await _context.SaveChangesAsync();
            return productType;
        }

        public async Task<List<ProductType>> GetAllAsync(ProductTypeQueryObject query)
        {
            var productTypes = _context.ProductType.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.ProductTypeCode))
            {
                productTypes = productTypes.Where(p => p.ProductTypeCode.Contains(query.ProductTypeCode));
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

            return await productTypes.ToListAsync();
        }

        public async Task<ProductType?> GetByIdAsync(int id)
        {
            return await _context.ProductType.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ProductTypeExists(string code)
        {
            return await _context.ProductType.AnyAsync(p => p.ProductTypeCode == code);
        }

        public async Task<ProductType?> UpdateProductTypeAsync(int id, UpdateProductTypeRequestDto prodTypeDto)
        {
            var existingProdType = await _context.ProductType.FirstOrDefaultAsync(p => p.Id == id);

            if (existingProdType == null)
            {
                return null;
            }

            existingProdType.ProductTypeCode = prodTypeDto.ProductTypeCode;
            existingProdType.Name = prodTypeDto.Name;
            existingProdType.Category = prodTypeDto.Category;
            existingProdType.Ingredients = prodTypeDto.Ingredients;

            await _context.SaveChangesAsync();
            return existingProdType;
        }

        public async Task UploadProductTypeAsync(IFormFile file)
        {
            using (var reader = new StreamReader(file.OpenReadStream()))
            {

                var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
                csvReader.Context.Configuration.HeaderValidated = null;
                csvReader.Context.RegisterClassMap<ProductTypeMap>();
                var records = csvReader.GetRecords<ProductType>();

                await _context.ProductType.AddRangeAsync(records);
                await _context.SaveChangesAsync();
            }
        }
    }
}