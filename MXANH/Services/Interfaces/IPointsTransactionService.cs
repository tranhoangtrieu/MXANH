using MXANH.Models;

namespace MXANH.Services.Interfaces
{
    public interface IPointsTransactionService
    {
        Task<PointsTransaction> GetAllPointsTransactionAsync();

        Task<IEnumerable<PointsTransaction>> GetTransactionsByUserIdAsync(int userId);
    }
}
