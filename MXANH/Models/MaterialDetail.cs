namespace MXANH.Models
{
    public class MaterialDetail
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string DetailName { get; set; } = string.Empty;
        public string? DetailDescription { get; set; }
        public decimal AdditionalPrice { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public virtual Material Material { get; set; } = null!;
    }
}
