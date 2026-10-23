using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.DTOs.Common;

namespace TMS_Proposal_Backend.Services.AuthService
{
    public interface ISignUpService
    {
        Task<ServiceResponse<AuthResponseDTO>> SignUpAsync(SignUpDTO signUpDTO);
    }
}
