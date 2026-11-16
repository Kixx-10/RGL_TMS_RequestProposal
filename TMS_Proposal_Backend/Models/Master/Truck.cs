using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS_Proposal_Backend.Models.Master
{
    public class Truck
    {
        [Key]
        public Guid TruckId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string TruckNo { get; set; } = string.Empty;


        [Required]
        [MaxLength(20)]
        public string JobType { get; set; } = string.Empty; // inbound or outbound


        [Required]
        public DateTime ETA { get; set; } = DateTime.UtcNow.Date;

        [Required]
        public Guid RequestId { get; set; }

        [ForeignKey("RequestId")]
        public Request? Request { get; set; }

        [Required]
        public DateTime CreatedDated { get; set; } = DateTime.UtcNow.Date;

        public DateTime? UpdatedDate { get; set; }

    }
}
