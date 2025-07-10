namespace MXANH.DTO.Request.AddressesRequestDTO
{
    public class CreateAddressDTO
    {
        public int UserId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string? PostalCode { get; set; }
        public bool IsDefault { get; set; }

    }
}
