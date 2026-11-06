namespace TMS_Proposal_Backend.DTOs
{
    public class ResponseRequestDTO
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public DateTime JobDate { get; set; } = DateTime.UtcNow.Date;
        public List<ResponseTruckDTO> Trucks { get; set; } = new();
        public string? FileName { get; set; }
        public string? FileUrl { get; set; }
        public Guid CreatedByUserId { get; set; }

    }
    public class ResponseTruckDTO
    {
        public Guid TruckId { get; set; }
        public string TruckNo { get; set; } = string.Empty;
        public string JobType { get; set; } = string.Empty;
        public DateTime ETA { get; set; } = DateTime.UtcNow.Date;
    }
}
