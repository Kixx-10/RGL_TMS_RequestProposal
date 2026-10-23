using TMS_Proposal_Backend.Models.Auth;

namespace TMS_Proposal_Backend.Repository.AuthRepository
{
    public interface ISignInRepository
    {
        Task<User?> GetUserByEmailAsync(string email);
    }
}
