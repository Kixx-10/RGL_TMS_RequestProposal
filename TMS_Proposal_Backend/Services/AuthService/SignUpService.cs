using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.DTOs.Common;
using TMS_Proposal_Backend.Models.Auth;
using TMS_Proposal_Backend.Repository.AuthRepository;
using TMS_Proposal_Backend.Services.AuditsLogService;
using TMS_Proposal_Backend.Services.TokenService;

namespace TMS_Proposal_Backend.Services.AuthService
{
    public class SignUpService : ISignUpService
    {
        private readonly IMapper _mapper;
        private readonly ISignUpRepository _repo;
        private readonly ITokenService _tokenService;
        IAuditsLogService _auditsLogService;
        public SignUpService(IMapper mapper, ISignUpRepository repo, ITokenService tokenService, IAuditsLogService auditsLogService)
        {
            _mapper = mapper;
            _repo = repo;
            _tokenService = tokenService;
            _auditsLogService = auditsLogService;
        }
        public async Task<ServiceResponse<AuthResponseDTO>> SignUpAsync(SignUpDTO signUpDTO)
        {
            bool existedEmail = await _repo.IsExixtedEmailAsync(signUpDTO.Email);
            if (existedEmail)
            {
                return ServiceResponse<AuthResponseDTO>.Failure("EMAIL_ALREADY_EXIST", "Registration fail!, email is already use");
            }
            try
            {
                var user = _mapper.Map<User>(signUpDTO);
                user.Password = BCrypt.Net.BCrypt.HashPassword(signUpDTO.Password);
                var result = await _repo.AddUserAsync(user);
                if (result)
                {
                    var authResponse = new AuthResponseDTO
                    {
                        User = _mapper.Map<UserResponseDTO>(user),
                        Token = _tokenService.CreateToken(user)
                    };
                    //Log ဖမ်းတာ
                    await _auditsLogService.LogAsync(
                    user.Id,
                    "User.SignUp",
                    new { user.Email });

                    return ServiceResponse<AuthResponseDTO>.Success(authResponse, "User registered successfully!");
                }
                return ServiceResponse<AuthResponseDTO>.Failure("REGISTRATION_FAILED", "Could not save user data.");
            }
            catch (DbUpdateException ex)
            {
                return ServiceResponse<AuthResponseDTO>.Failure("DATABASE_ERROR", $"Database error occurred: {ex.InnerException?.Message ?? ex.Message}");
            }
            catch (Exception ex)
            {
                return ServiceResponse<AuthResponseDTO>.Failure("SERVER_ERROR", ex.Message);
            }
        }
    }
}