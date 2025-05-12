namespace MXANH.DTO.Request
{
    public class MaterialItem
    {
        public string Material { get; set; } // e.g., "bottle", "paper"
        public double WeightKg { get; set; }
        public string Condition { get; set; } // "new", "used", "damaged"
        public double PercentageOfNew { get; set; } // e.g., 80.0 for 80% new
    }

    public class EarningsCalculationRequest
    {
        public List<MaterialItem> Items { get; set; }
    }
}
