using SalesApi.Dtos;
using SalesApi.Models;

namespace SalesApi.Mappers
{
    public static class ProductDtoMapper
    {
        public static ProductDto ToProductDto(this Product productModel)
        {
            return new ProductDto
            {
                Id = productModel.Id,
                ProductCode = productModel.ProductCode,
                Size = productModel.Size,
                Price = productModel.Price
            };
        }

        public static Product CreateProductRequestDto(this CreateProductRequestDto productDto)
        {
            return new Product
            {
                ProductCode = productDto.ProductCode,
                ProductTypeCode = productDto.ProductTypeCode,
                Size = productDto.Size,
                Price = productDto.Price,
            };
        }
    }
}