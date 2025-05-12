using MXANH.DTO.Request;
using MXANH.DTO.Response;

namespace MXANH.Services.Interfaces
{
    public interface IEarningsCalculationService
    {
        Task<EarningsCalculationResponse> CalculateEarningsAndPointsAsync(EarningsCalculationRequest request);
    }
}
