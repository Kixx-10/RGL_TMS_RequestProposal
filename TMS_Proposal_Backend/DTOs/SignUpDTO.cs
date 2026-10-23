namespace TMS_Proposal_Backend.DTOs
{
    public class SignUpDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
    }
}
