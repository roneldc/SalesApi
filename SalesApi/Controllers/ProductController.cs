using Microsoft.AspNetCore.Mvc;
using SalesApi.Dtos;
using SalesApi.Helpers;
using SalesApi.Interface;
using SalesApi.Mappers;
using SalesApi.Models;
using SalesApi.Repository;

namespace SalesApi.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductTypeRepository _productTypeRepo;
        public ProductController(IProductRepository productRepo, IProductTypeRepository productTypeRepository)
        {
            _productRepo = productRepo;
            _productTypeRepo = productTypeRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var products = await _productRepo.GetAllAsync(query);

            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var products = await _productRepo.GetByIdAsync(id);

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductType([FromBody] CreateProductRequestDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            if (!await _productTypeRepo.ProductTypeExists(productDto.ProductTypeCode))
                return BadRequest(new { message = "Could not find Product Type code.", error = ModelState });

            var productModel = productDto.CreateProductRequestDto();

            await _productRepo.CreateProductAsync(productModel);

            return CreatedAtAction(nameof(GetById), new { id = productModel.Id }, productModel.ToProductDto());
        }

        [HttpPost("import")]
        public async Task<IActionResult> PostProduct(IFormFile file)
        {
            if (!FileHelper.IsValidCsvFile(file))
                return BadRequest(new { message = "File upload failed. Invalid file type, only .csv are allowed." });

            try
            {
                await _productRepo.UploadProductAsync(file);
                return Ok(new { message = "File processed successfully.", fileName = file.FileName, ProcessedDateTime = DateTime.Now });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, new { message = "An error occured while processing the file.", error = ex.Message });
            }
        }
    }
}