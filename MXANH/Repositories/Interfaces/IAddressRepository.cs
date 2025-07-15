using MXANH.Models;

namespace MXANH.Repositories.Interfaces
{
    public interface IAddressRepository
    {
        Task<Address> GetAddressByIdAsync(int id);
        Task<IEnumerable<Address>> GetAddressesByUserIdAsync(int userId);
        Task AddAddressAsync(Address address);
        Task UpdateAddressAsync(Address address);
        Task DeleteAddressAsync(int id);
    }
}
