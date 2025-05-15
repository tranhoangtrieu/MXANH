using MXANH.Models;
using MXANH.Repositories.Interfaces;
using MXANH.Services.Interfaces;

namespace MXANH.Services.Implementations
{
    public class PointsTransactionService : IPointsTransactionService
    {
        private readonly IPointsTransactionRepository _pointsTransactionRepository;

        public PointsTransactionService(IPointsTransactionRepository pointsTransactionRepository)
        {
            _pointsTransactionRepository = pointsTransactionRepository;
        }

     
        async Task<PointsTransaction> IPointsTransactionService.GetAllPointsTransactionAsync()
        {
          return (PointsTransaction)await _pointsTransactionRepository.GetAllTransactionsAsync();
        }

        public async Task<IEnumerable<PointsTransaction>> GetTransactionsByUserIdAsync(int userId)
        {
            return await _pointsTransactionRepository.GetTransactionsByUserIdAsync(userId);
        }


    }
}
