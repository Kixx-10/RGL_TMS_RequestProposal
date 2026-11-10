using System.ComponentModel.DataAnnotations;

namespace TMS_Proposal_Backend.DTOs
{
    public class UpdateRequestDTO
    {
        [MaxLength(100)]
        public string? CustomerName { get; set; }

        public DateTime? JobDate { get; set; }

        public List<CreateRequestAttachmentDTO>? Attachments { get; set; }

        public List<CreateTruckDTO>? Trucks { get; set; }
    }
}