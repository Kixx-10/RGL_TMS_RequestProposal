using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS_Proposal_Backend.Models.Auth;
using TMS_Proposal_Backend.Models.Master;

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


        [MaxLength(100)]
        public string? FileName { get; set; }

        [MaxLength(255)]
        public string? FileUrl { get; set; }

        [Required]
        public Guid CreatedByUserId { get; set; } 

        [ForeignKey("CreatedByUserId")]
        public User? CreatedByUser { get; set; }

        [Required]
        public DateTime CreatedDated { get; set; } = DateTime.UtcNow.Date;

        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Truck> Trucks { get; set; }=new List<Truck>();
    }
}
