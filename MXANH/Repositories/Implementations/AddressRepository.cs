using Microsoft.EntityFrameworkCore;
using MXANH.Models;
using MXANH.Repositories.Interfaces;

namespace MXANH.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _context;
        public AddressRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Address> GetAddressByIdAsync(int id)
        {
            return await _context.Addresses.FindAsync(id);
        }
        public async Task<IEnumerable<Address>> GetAddressesByUserIdAsync(int userId)
        {
            return await _context.Addresses.Where(a => a.UserId == userId).ToListAsync();
        }
        public async Task AddAddressAsync(Address address)
        {
            if(_context.Addresses.Any(a => a.UserId == address.UserId && a.Street == address.Street && a.City == address.City))
            {
                throw new InvalidOperationException("An address with the same user, street, and city already exists.");
            }
            if (_context.Addresses.Where(a => a.UserId == address.UserId).ToListAsync() ==   null)
            {
                address.IsDefault = true;

            }


            await _context.Addresses.AddAsync(address);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAddressAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAddressAsync(int id)
        {
            var address = await GetAddressByIdAsync(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
        }
    }
}
