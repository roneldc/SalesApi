using Microsoft.AspNetCore.Mvc;
using SalesApi.Helpers;
using SalesApi.Interface;

namespace SalesApi.Controllers
{
    /// <summary>
    /// Controller for handling Order Detail related requests.
    /// </summary>
    [ApiController]
    [Route("api/orders")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepository _orderRepo;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderDetailController"/> class.
        /// </summary>
        /// <param name="orderDetailRepository">The order detail repository.</param>
        public OrderDetailController(IOrderDetailRepository orderDetailRepository)
        {
            _orderRepo = orderDetailRepository;
        }

        /// <summary>
        /// Import the order details file for processing.
        /// </summary>
        /// <param name="file">The csv file containing order details.</param>
        /// <returns>A response indicating the result of the file processing.</returns>
        [HttpPost("/details/import")]
        public async Task<IActionResult> PostOrderDetail(IFormFile file)
        {
            if (!FileHelper.IsValidCsvFile(file))
                return BadRequest(new { message = "File upload failed. Invalid file type, only .csv are allowed." });

            try
            {
                await _orderRepo.UploadOrderDetailsAsync(file);
                return Ok(new { message = "File processed successfully.", fileName = file.FileName, ProcessedDateTime = DateTime.Now });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occured while processing the file.", error = ex.Message });
            }
        }
    }
}