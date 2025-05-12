namespace MXANH.DTO.Response
{
    public class EarningsCalculationResult
    {
        public required string Material { get; set; }
        public double Earnings { get; set; }
        public int Points { get; set; }
    }

    public class EarningsCalculationResponse
    {
        public required List<EarningsCalculationResult> Results { get; set; }
        public double TotalEarnings { get; set; }
        public int TotalPoints { get; set; }
        public string? Message { get; set; }
    }
}
