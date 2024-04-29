using Microsoft.AspNetCore.Mvc;
using SalesApi.Dtos;
using SalesApi.Helpers;
using SalesApi.Interface;
using SalesApi.Mappers;
using SalesApi.Models;
using SalesApi.Repository;

namespace SalesApi.Controllers
{
    /// <summary>
    /// Controller for handling Product related requests.
    /// </summary>
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductTypeRepository _productTypeRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productRepo">The product repository.</param>
        /// <param name="productTypeRepository">The product type repository.</param>
        public ProductController(IProductRepository productRepo, IProductTypeRepository productTypeRepository)
        {
            _productRepo = productRepo;
            _productTypeRepo = productTypeRepository;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <param name="query">The query parameters for filtering products.</param>
        /// <returns>A list of products.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var products = await _productRepo.GetAllAsync(query);

            return Ok(products);
        }

        /// <summary>
        /// Gets the product by id.
        /// </summary>
        /// <param name="id">The product id.</param>
        /// <returns>The product associated with the id.</returns>
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var products = await _productRepo.GetByIdAsync(id);

            return Ok(products);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productDto">The product data transfer object.</param>
        /// <returns>The created product.</returns>

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

        /// <summary>
        /// Import the product file for processing.
        /// </summary>
        /// <param name="file">The csv file containing product data.</param>
        /// <returns>A response indicating the result of the file processing.</returns>

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