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
    /// <summary>
    /// Controller for handling Product Type related requests.
    /// </summary>
    [ApiController]
    [Route("api/product/types")]
    public class ProductTypeController : ControllerBase
    {
        private readonly IProductTypeRepository _productTypeRepo;
        /// <summary>
        /// Initializes a new instance of the <see cref="ProductTypeController"/> class.
        /// </summary>
        /// <param name="productTypeRepository">The product type repository.</param>
        public ProductTypeController(IProductTypeRepository productTypeRepository)
        {
            _productTypeRepo = productTypeRepository;
        }

        /// <summary>
        /// Gets all product types.
        /// </summary>
        /// <param name="query">The query parameters for filtering product types.</param>
        /// <returns>A list of product types.</returns>

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductTypeQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var products = await _productTypeRepo.GetAllAsync(query);

            return Ok(products);
        }

        /// <summary>
        /// Gets the product type by id.
        /// </summary>
        /// <param name="id">The product type id.</param>
        /// <returns>The product type associated with the id.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetProductTypeBydId([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var productTypes = await _productTypeRepo.GetByIdAsync(id);

            return Ok(productTypes);
        }

        /// <summary>
        /// Creates a new product type.
        /// </summary>
        /// <param name="productTypeDto">The product type data transfer object.</param>
        /// <returns>The created product type.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateProductType([FromBody] CreateProductTypeRequestDto productTypeDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var productTypeModel = productTypeDto.CreateProductTypeRequestDto();

            await _productTypeRepo.CreateProductTypeAsync(productTypeModel);

            return CreatedAtAction(nameof(GetProductTypeBydId), new { id = productTypeModel.Id }, productTypeModel.ToProductTypeDto());
        }

        /// <summary>
        /// Import the product type file for processing.
        /// </summary>
        /// <param name="file">The csv file containing product type data.</param>
        /// <returns>A response indicating the result of the file processing.</returns>
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

        /// <summary>
        /// Updates a product type.
        /// </summary>
        /// <param name="id">The product type id.</param>
        /// <param name="productTypeDto">The product type data transfer object.</param>
        /// <returns>The updated product type.</returns>
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


        /// <summary>
        /// Deletes a product type.
        /// </summary>
        /// <param name="id">The product type id.</param>
        /// <returns>A response indicating the result of the deletion.</returns>
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