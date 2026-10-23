using AutoMapper;
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.DTOs.Common;
using TMS_Proposal_Backend.Repository.AuthRepository;
using TMS_Proposal_Backend.Services.AuditsLogService;
using TMS_Proposal_Backend.Services.TokenService;

namespace TMS_Proposal_Backend.Services.AuthService
{
    public class SignInService : ISignInService
    {
        private readonly ISignInRepository _repo;
        private readonly IMapper _mapper;
        IAuditsLogService _auditsLogService;

        private readonly ITokenService _tokenService;

        public SignInService(IMapper mapper, ISignInRepository repo, ITokenService tokenService, IAuditsLogService auditsLogService)
        {
            _mapper = mapper;
            _repo = repo;
            _tokenService = tokenService;
            _auditsLogService = auditsLogService;
        }

        public async Task<ServiceResponse<AuthResponseDTO>> SignInAsync(SignInDTO signInDto)
        {
            var dbUser = await _repo.GetUserByEmailAsync(signInDto.Email);

            if (dbUser == null)
            {
                return ServiceResponse<AuthResponseDTO>.Failure("USER_NOT_FOUND", "Email Not Found");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(signInDto.Password, dbUser.Password);
            if (!isPasswordValid)
            {
                return ServiceResponse<AuthResponseDTO>.Failure("INVALID_PASSWORD", "Invalid Password");
            }
            //for auditlogs
            await _auditsLogService.LogAsync(
            dbUser.Id,
            "User.SignIn",
            new { dbUser.Email });

            var authResponse = new AuthResponseDTO
            {
                User = _mapper.Map<UserResponseDTO>(dbUser),
                Token = _tokenService.CreateToken(dbUser)
            };

            return ServiceResponse<AuthResponseDTO>.Success(authResponse, "Login successfully!");
        }
    }
}