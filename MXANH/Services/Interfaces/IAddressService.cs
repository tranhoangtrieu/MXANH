using MXANH.DTO.Request.AddressesRequestDTO;
using MXANH.Models;

namespace MXANH.Services.Interfaces
{
    public interface IAddressService
    {
        Task<Address> GetAddressByIdAsync(int id);
        Task<IEnumerable<Address>> GetAddressesByUserIdAsync(int userId);
        Task AddAddressAsync(CreateAddressDTO address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(int id);
    }
}
