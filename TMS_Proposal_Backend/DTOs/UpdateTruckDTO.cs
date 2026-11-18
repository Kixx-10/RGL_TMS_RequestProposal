
using System.ComponentModel.DataAnnotations;

namespace TMS_Proposal_Backend.DTOs
{
    public class UpdateTruckDTO
    {
        [Required]
        [MaxLength(50)]
        public string TruckNo { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string JobType { get; set; } = string.Empty;

        [Required]
        public DateTime ETA { get; set; }

        public bool? IsArrived { get; set; }
    }
}
