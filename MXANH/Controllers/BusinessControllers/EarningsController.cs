using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MXANH.DTO.Request;
using MXANH.Services.Interfaces;

namespace MXANH.Controllers.BusinessControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EarningsController : ControllerBase
    {
        private readonly IEarningsCalculationService _earningsService;

        public EarningsController(IEarningsCalculationService earningsService)
        {
            _earningsService = earningsService;
        }

        [HttpPost("calculate")]
        [Authorize] // Optional: Secure the endpoint with JWT
        public async Task<IActionResult> CalculateEarningsAndPoints([FromBody] EarningsCalculationRequest request)
        {
            var response = await _earningsService.CalculateEarningsAndPointsAsync(request);
            return Ok(response);
        }
    }
}
