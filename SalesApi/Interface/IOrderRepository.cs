using SalesApi.Dtos;
using SalesApi.Helpers;

namespace SalesApi.Interface
{
    public interface IOrderRepository
    {
        Task<List<OrderDto>> GetAllAsync(OrderQueryObject query);
        Task<OrderDto?> GetByProductCodeAsync(string productCode);
        Task UploadOrderAsync(IFormFile file);

    }
}