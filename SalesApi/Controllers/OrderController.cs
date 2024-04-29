using Microsoft.AspNetCore.Mvc;
using SalesApi.Helpers;
using SalesApi.Interface;

namespace SalesApi.Controllers
{
    /// <summary>
    /// Controller for handling Order related requests.
    /// </summary>
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderRepository">The order repository.</param>
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepo = orderRepository;
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <param name="query">The query parameters for filtering orders.</param>
        /// <returns>A list of orders.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] OrderQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var orders = await _orderRepo.GetAllAsync(query);

            return Ok(orders);
        }

        /// <summary>
        /// Gets the order by product code.
        /// </summary>
        /// <param name="code">The product code.</param>
        /// <returns>The order associated with the product code.</returns>
        [HttpGet]
        [Route("{code:required}")]
        public async Task<IActionResult> GetById([FromRoute] string code)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var orders = await _orderRepo.GetByProductCodeAsync(code);

            return Ok(orders);
        }

        /// <summary>
        /// Import the order data for processing.
        /// </summary>
        /// <param name="file">The csv file containing order data.</param>
        /// <returns>A response indicating the result of the file processing.</returns>
        [HttpPost("import")]
        public async Task<IActionResult> PostOrder(IFormFile file)
        {
            if (!FileHelper.IsValidCsvFile(file))
                return BadRequest(new { message = "File upload failed. Invalid file type, only .csv are allowed." });

            try
            {
                await _orderRepo.UploadOrderAsync(file);
                return Ok(new { message = "File processed successfully.", fileName = file.FileName, ProcessedDateTime = DateTime.Now });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occured while processing the file.", error = ex.Message });
            }
        }
    }
}