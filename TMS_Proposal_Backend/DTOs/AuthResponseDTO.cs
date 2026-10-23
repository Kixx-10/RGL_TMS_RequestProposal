namespace TMS_Proposal_Backend.DTOs
{
    public class AuthResponseDTO
    {
        public UserResponseDTO User { get; set; } = null!;
        public string Token { get; set; } = string.Empty;
    }
}
