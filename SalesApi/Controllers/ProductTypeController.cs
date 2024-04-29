using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Dtos;
using SalesApi.Helpers;
using SalesApi.Interface;
using SalesApi.Mappers;

namespace SalesApi.Controllers
{
    [ApiController]
    [Route("api/product/types")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeRepository _productTypeRepo;
        public ProductTypeController(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepo = productTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductTypeQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var products = await _productTypeRepo.GetAllAsync(query);

            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetProductTypeBydId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var productTypes = await _productTypeRepo.GetByIdAsync(id);

            return Ok(productTypes);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductType([FromBody] CreateProductTypeRequestDto productTypeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var productTypeModel = productTypeDto.CreateProductTypeRequestDto();

            await _productTypeRepo.CreateProductTypeAsync(productTypeModel);

            return CreatedAtAction(nameof(GetProductTypeBydId), new { id = productTypeModel.Id }, productTypeModel.ToProductTypeDto());
        }

        [HttpPost("import")]
        public async Task<IActionResult> PostProductType(IFormFile file)
        {
            if (!FileHelper.IsValidCsvFile(file))
                return BadRequest(new { message = "File upload failed. Invalid file type, only .csv are allowed." });

            try
            {
                await _productTypeRepo.UploadProductTypeAsync(file);
                return Ok(new { message = "File processed successfully.", fileName = file.FileName, ProcessedDateTime = DateTime.Now });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occured while processing the file.", error = ex.Message });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateProductType([FromRoute] int id, [FromBody] UpdateProductTypeRequestDto productTypeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var productTypeModel = await _productTypeRepo.UpdateProductTypeAsync(id, productTypeDto);

            if (productTypeModel == null)
            {
                return NotFound(new { message = "Product type not found.", error = ModelState });
            }

            return CreatedAtAction(nameof(GetProductTypeBydId), new { id = productTypeModel.Id }, productTypeModel.ToProductTypeDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteProductType([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var productTypeModel = await _productTypeRepo.DeleteProductTypeAsync(id);

            if (productTypeModel == null)
            {
                return NotFound(new { message = "Product type not found.", error = ModelState });
            }

            return Ok(new { message = $"Product type removed successfully. ProductType Id {id}" });
        }
    }
}