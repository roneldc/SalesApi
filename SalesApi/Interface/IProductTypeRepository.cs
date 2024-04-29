using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesApi.Dtos;
using SalesApi.Helpers;
using SalesApi.Models;

namespace SalesApi.Interface
{
    public interface IProductTypeRepository
    {
        Task<List<ProductType>> GetAllAsync(ProductTypeQueryObject query);
        Task<ProductType?> GetByIdAsync(int id);
        Task<ProductType> CreateProductTypeAsync(ProductType productTypeModel);
        Task<ProductType?> UpdateProductTypeAsync(int id, UpdateProductTypeRequestDto productTypeDto);
        Task<ProductType?> DeleteProductTypeAsync(int id);
        Task UploadProductTypeAsync(IFormFile file);
        Task<bool> ProductTypeExists(string code);
    }
}