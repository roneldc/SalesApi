using SalesApi.Dtos;
using SalesApi.Helpers;
using SalesApi.Models;

namespace SalesApi.Interface
{
    public interface IProductRepository
    {
        Task<List<ProductDto>> GetAllAsync(ProductQueryObject query);
        Task<ProductDto?> GetByIdAsync(int id);
        Task<Product> CreateProductAsync(Product productModel);
        Task UploadProductAsync(IFormFile file);
    }
}