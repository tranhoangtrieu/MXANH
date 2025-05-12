using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MXANH.DTO.Request;
using MXANH.DTO.Response;
using MXANH.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MXANH.Services.Implementations
{
    public class EarningsCalculationService : IEarningsCalculationService
    {
        private readonly ILogger<EarningsCalculationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly Dictionary<string, MaterialRate> _materialRates;
        private readonly Dictionary<string, double> _conditionMultipliers;

        public EarningsCalculationService(ILogger<EarningsCalculationService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            // Load material rates and condition multipliers from configuration
            _materialRates = _configuration.GetSection("CalculationSettings:MaterialRates")
                .Get<Dictionary<string, MaterialRate>>() ?? new Dictionary<string, MaterialRate>();
            _conditionMultipliers = _configuration.GetSection("CalculationSettings:ConditionMultipliers")
                .Get<Dictionary<string, double>>() ?? new Dictionary<string, double>();
        }

        public async Task<EarningsCalculationResponse> CalculateEarningsAndPointsAsync(EarningsCalculationRequest request)
        {
            if (request?.Items == null || !request.Items.Any())
            {
                return new EarningsCalculationResponse
                {
                    Results = new List<EarningsCalculationResult>(),
                    TotalEarnings = 0,
                    TotalPoints = 0,
                    Message = "No items provided"
                };
            }

            var results = new List<EarningsCalculationResult>();
            double totalEarnings = 0;
            int totalPoints = 0;

            foreach (var item in request.Items)
            {
                // Get rates for the material (default if not found)
                var materialKey = item.Material?.ToLower() ?? "default";
                if (!_materialRates.TryGetValue(materialKey, out var rate))
                    rate = _materialRates["default"];

                // Get condition multiplier (default to 1.0 if not found)
                var conditionKey = item.Condition?.ToLower() ?? "used";
                if (!_conditionMultipliers.TryGetValue(conditionKey, out var conditionMultiplier))
                    conditionMultiplier = 1.0;

                // Calculate earnings and points
                double percentageOfNewMultiplier = item.PercentageOfNew / 100.0; // e.g., 80% -> 0.8
                double earnings = Math.Round(item.WeightKg * rate.EarningsPerKg * conditionMultiplier * percentageOfNewMultiplier, 2);
                int points = (int)Math.Round(item.WeightKg * rate.PointsPerKg * conditionMultiplier * percentageOfNewMultiplier);

                var result = new EarningsCalculationResult
                {
                    Material = item.Material,
                    Earnings = earnings,
                    Points = points
                };
                results.Add(result);

                totalEarnings += earnings;
                totalPoints += points;

                // Log calculation for debugging
                _logger.LogInformation(
                    "Calculated for {Material}: Earnings={Earnings}, Points={Points}, Weight={Weight}kg, Condition={Condition}, PercentageOfNew={PercentageOfNew}%",
                    item.Material, earnings, points, item.WeightKg, item.Condition, item.PercentageOfNew);
            }

            return await Task.FromResult(new EarningsCalculationResponse
            {
                Results = results,
                TotalEarnings = totalEarnings,
                TotalPoints = totalPoints,
                Message = "Calculations completed successfully"
            });
        }
    }

    public class MaterialRate
    {
        public double EarningsPerKg { get; set; }
        public double PointsPerKg { get; set; }
    }
}
