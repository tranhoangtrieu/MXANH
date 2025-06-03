using Microsoft.EntityFrameworkCore;
using MXANH.Models;
using MXANH.Repositories.Interfaces;

namespace MXANH.Repositories.Implementations
{
    public class PointsTransactionRepository : IPointsTransactionRepository
    {
        private readonly AppDbContext _context;
        public PointsTransactionRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddTransactionAsync(PointsTransaction transaction)
        {
            await _context.PointsTransactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }
        // Uncomment if you need to implement these methods
        
        public async Task<PointsTransaction> GetTransactionByIdAsync(int id)
        {
            return await _context.PointsTransactions.FindAsync(id);
        }
        public async Task<IEnumerable<PointsTransaction>> GetAllTransactionsAsync()
        {
            return await _context.PointsTransactions.ToListAsync();
        }
        public async Task<IEnumerable<PointsTransaction>> GetTransactionsByUserIdAsync(int userId)
        {
            return await _context.PointsTransactions.Where(t => t.UserId == userId).ToListAsync();
        }
        
    }
}
