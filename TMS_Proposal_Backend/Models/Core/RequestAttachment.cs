using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS_Proposal_Backend.Models
{
    public class RequestAttachment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid RequestId { get; set; }

        [ForeignKey("RequestId")]
        public Request? Request { get; set; }

        [Required]
        [MaxLength(255)]
        public string FileName { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string FileUrl { get; set; } = string.Empty; // Server တွင် သိမ်မည့် GUID  Path

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}