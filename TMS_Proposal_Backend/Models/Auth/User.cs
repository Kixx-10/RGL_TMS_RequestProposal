using System.ComponentModel.DataAnnotations;
using TMS_Proposal_Backend.Models.Audits;

namespace TMS_Proposal_Backend.Models.Auth
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }= Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        public string Email {  get; set; } = string.Empty;

        [Required]
        [MaxLength(255)]
        [MinLength(3)]
        public string Password {  get; set; }= string.Empty;

        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public DateTime CreatedDated { get; set; } = DateTime.UtcNow.Date;
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
        public virtual ICollection<AuditLogs>AuditLogs { get; set; }= new List<AuditLogs>();
    }
}
