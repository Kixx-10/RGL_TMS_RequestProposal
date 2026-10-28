namespace TMS_Proposal_Backend.DTOs
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public DateTime CreatedDated { get; set; }
    }
}
