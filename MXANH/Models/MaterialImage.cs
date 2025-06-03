namespace MXANH.Models
{
    public class MaterialImage
    {
        public int Id { get; set; }

        public int MaterialId { get; set; } 
        public string ImageUrl { get; set; }

        public Material Material { get; set; } 
    }
}
