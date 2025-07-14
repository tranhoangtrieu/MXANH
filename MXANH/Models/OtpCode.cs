namespace MXANH.Models
{
    public class OtpCode
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
        public DateTime ExpireAt { get; set; }
        public bool IsUsed { get; set; }
        public int FailedAttempts { get; set; }
        public DateTime? LockedUntil { get; set; } = null;

    }
}
