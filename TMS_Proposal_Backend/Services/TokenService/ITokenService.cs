using TMS_Proposal_Backend.Models.Auth;

namespace TMS_Proposal_Backend.Services.TokenService
{
    public interface ITokenService
    {
        String CreateToken(User user);
    }
}
