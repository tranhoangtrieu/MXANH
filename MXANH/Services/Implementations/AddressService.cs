using MXANH.DTO.Request.AddressesRequestDTO;
using MXANH.Models;
using MXANH.Repositories.Interfaces;
using MXANH.Services.Interfaces;

namespace MXANH.Services.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        public async Task<Address> GetAddressByIdAsync(int id)
        {
            return await _addressRepository.GetAddressByIdAsync(id);
        }
        public async Task<IEnumerable<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await _addressRepository.GetAddressesByUserIdAsync(userId);
        }
        public async Task AddAddressAsync(CreateAddressDTO address)
        {
            var new_address = new Address
            {
                UserId = address.UserId,
                Street = address.Street,
                City = address.City,
                IsDefault = address.IsDefault,
            };
            await _addressRepository.AddAddressAsync(new_address);
        }
        public async Task UpdateAddressAsync(Address address)
        {
            await _addressRepository.UpdateAddressAsync(address);
        }
        public async Task DeleteAddressAsync(int id)
        {
            await _addressRepository.DeleteAddressAsync(id);

        }

    }
}
