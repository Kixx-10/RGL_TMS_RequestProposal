using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TMS_Proposal_Backend.Models.Auth;

namespace TMS_Proposal_Backend.Models.Audits
{
    public class AuditLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LogId { get; set; }

        [Required]
        public DateTime LogTime { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User {  get; set; }


        [Required, MaxLength(20)]
        public string LogIPAddr { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        [Column(TypeName = "varchar(50)")]
        public string Activity { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Inputted { get; set; }
    }
}
