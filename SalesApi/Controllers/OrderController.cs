using Microsoft.AspNetCore.Mvc;
using SalesApi.Helpers;
using SalesApi.Interface;

namespace SalesApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;
        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepo = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] OrderQueryObject query)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var orders = await _orderRepo.GetAllAsync(query);

            return Ok(orders);
        }

        [HttpGet]
        [Route("{code:required}")]
        public async Task<IActionResult> GetById([FromRoute] string code)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid request.", error = ModelState });

            var orders = await _orderRepo.GetByProductCodeAsync(code);

            return Ok(orders);
        }

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