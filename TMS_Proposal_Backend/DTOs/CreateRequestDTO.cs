namespace TMS_Proposal_Backend.DTOs
{
    public class CreateRequestDTO
    {
        public string CustomerName { get; set; } = string.Empty;
        public DateTime JobDate { get; set; } = DateTime.UtcNow.Date;
        public string JobType { get; set; } = string.Empty;
        public string CarNo { get; set; } = string.Empty;
        public string DriverName { get; set; } = string.Empty;
        public string DriverLicenseNo { get; set; } = string.Empty;
        public DateTime RegistrationDateTime { get; set; } = DateTime.UtcNow.Date;

    }
}
