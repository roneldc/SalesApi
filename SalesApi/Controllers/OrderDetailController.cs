using Microsoft.AspNetCore.Mvc;
using SalesApi.Helpers;
using SalesApi.Interface;

namespace SalesApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderDetailController : ControllerBase
    {
        private readonly IOrderDetailRepository _orderRepo;
        public OrderDetailController(IOrderDetailRepository orderDetailRepository)
        {
            _orderRepo = orderDetailRepository;
        }

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