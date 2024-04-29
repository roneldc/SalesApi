using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesApi.Dtos;
using SalesApi.Models;

namespace SalesApi.Mappers
{
    public static class ProductTypeMapper
    {
        public static ProductTypeDto ToProductTypeDto(this ProductType productTypeModel)
        {
            return new ProductTypeDto
            {
                Id = productTypeModel.Id,
                ProductTypeCode = productTypeModel.ProductTypeCode,
                Name = productTypeModel.Name,
                Category = productTypeModel.Category,
                Ingredients = productTypeModel.Ingredients,
            };
        }

        public static ProductType CreateProductTypeRequestDto(this CreateProductTypeRequestDto productTypeDto)
        {
            return new ProductType
            {
                ProductTypeCode = productTypeDto.ProductTypeCode,
                Name = productTypeDto.Name,
                Category = productTypeDto.Category,
                Ingredients = productTypeDto.Ingredients,
            };
        }
    }
}