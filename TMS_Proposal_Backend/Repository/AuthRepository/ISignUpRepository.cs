using TMS_Proposal_Backend.Models.Auth;

namespace TMS_Proposal_Backend.Repository.AuthRepository
{
    public interface ISignUpRepository
    {
        Task<bool> IsExixtedEmailAsync(string email);
        Task<bool> AddUserAsync(User user);
        
    }
}
