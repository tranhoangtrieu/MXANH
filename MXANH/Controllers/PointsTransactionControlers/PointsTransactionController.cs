using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MXANH.Services.Implementations;
using MXANH.Services.Interfaces;

namespace MXANH.Controllers.PointsTransactionControlers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsTransactionController : ControllerBase
    {

        private readonly IPointsTransactionService _pointTransactionService;

        public PointsTransactionController(IPointsTransactionService pointTransactionService)
        {
            _pointTransactionService = pointTransactionService;
        }

        [HttpGet("{id}/points-history")]
        public async Task<IActionResult> GetPointsHistory(int id)
        {
            var history = await _pointTransactionService.GetTransactionsByUserIdAsync(id);
            return Ok(history);

        }
    }
}
