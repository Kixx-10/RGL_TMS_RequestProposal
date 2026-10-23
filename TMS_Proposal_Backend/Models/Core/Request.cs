using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS_Proposal_Backend.Models.Auth;

namespace TMS_Proposal_Backend.Models
{
    public class Request
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public DateTime JobDate { get; set; } = DateTime.UtcNow.Date;

        [Required]
        [MaxLength(20)]
        public string JobType { get; set; } = string.Empty; // inbound or outbound

        [Required]
        [MaxLength(20)]
        public string CarNo { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string DriverName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string DriverLicenseNo { get; set; } = string.Empty;

        [Required]
        public DateTime RegistrationDateTime { get; set; } = DateTime.UtcNow.Date;

        [Required]
        public DateTime CreatedDated { get; set; } = DateTime.UtcNow.Date;

        public DateTime? UpdatedDate { get; set; }

        [Required]
        public Guid CreatedByUserId { get; set; } 

        [ForeignKey("CreatedByUserId")]
        public User? CreatedByUser { get; set; }
    }
}
