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
        public Guid RequestId { get; set; }

        [ForeignKey("RequestId")]
        public Request? Request { get; set; }

    }
}
