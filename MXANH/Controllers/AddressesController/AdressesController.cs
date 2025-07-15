using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MXANH.DTO.Request.AddressesRequestDTO;
using MXANH.Models;
using MXANH.Services.Implementations;
using MXANH.Services.Interfaces;

namespace MXANH.Controllers.AddressesController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdressesController : ControllerBase
    {
       private readonly IAddressService _addressService;
        public AdressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        [HttpGet("{UserId}")]
        public async Task<IActionResult> GetAddressesByUserId(int UserId)
        {
            var addresses = await _addressService.GetAddressesByUserIdAsync(UserId);
            if (addresses == null || !addresses.Any())
            {
                return NotFound("No addresses found for this user.");
            }
            return Ok(addresses);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(int UserId, [FromBody] CreateAddressDTO address)
        {
            if (address == null)
            {
                return BadRequest("Address data is null");
            }
            await _addressService.AddAddressAsync(address);
            return CreatedAtAction(nameof(GetAddressesByUserId), new { UserId = UserId }, address);
        }

    }
}
