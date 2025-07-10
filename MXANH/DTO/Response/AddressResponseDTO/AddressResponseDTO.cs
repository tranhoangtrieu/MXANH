namespace MXANH.DTO.Response.AddressResponseDTO
{
    public class AddressResponseDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string? PostalCode { get; set; }
        public bool IsDefault { get; set; } = false; // Default is not default

    }
}
