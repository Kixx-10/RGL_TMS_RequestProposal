namespace TMS_Proposal_Backend.DTOs
{
    public class ResponseRequestDTO
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime JobDate { get; set; } = DateTime.UtcNow.Date;
        public string JobType { get; set; } = string.Empty;
        public string CarNo { get; set; } = string.Empty;
        public DateTime ETA { get; set; } = DateTime.UtcNow.Date;
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public Guid CreatedByUserId { get; set; }

    }
}
