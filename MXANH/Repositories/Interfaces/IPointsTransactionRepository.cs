using MXANH.Models;

namespace MXANH.Repositories.Interfaces
{
    public interface IPointsTransactionRepository
    {
        //Task<PointsTransaction> GetTransactionByIdAsync(int id);
        Task<IEnumerable<PointsTransaction>> GetAllTransactionsAsync();
        Task<IEnumerable<PointsTransaction>> GetTransactionsByUserIdAsync(int userId);
        Task AddTransactionAsync(PointsTransaction transaction);
    }
}
