using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalesApi.Interface
{
    public interface IOrderDetailRepository
    {
        Task UploadOrderDetailsAsync(IFormFile file);
    }
}