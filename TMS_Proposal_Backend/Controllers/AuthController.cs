using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.Services.AuthService;

namespace TMS_Proposal_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISignUpService _signUpService;
        private readonly ISignInService _signInService;
        private readonly IValidator<SignUpDTO> _signUpValidator;
        private readonly IValidator<SignInDTO> _signInValidator;
        public AuthController(ISignUpService signUpService, ISignInService signInService, IValidator<SignUpDTO> signUpValidator,
            IValidator<SignInDTO> signInValidator)
        {
            _signUpService = signUpService;
            _signInService = signInService;
            _signUpValidator = signUpValidator;
            _signInValidator = signInValidator;
        }

        //SignUp API
        [HttpPost("SignUp")]
        public async Task<ActionResult<AuthResponseDTO>> SignUp([FromBody] SignUpDTO signUpDTO)
        {
            var validation = await _signUpValidator.ValidateAsync(signUpDTO);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.Select(error => new
                {
                    field = error.PropertyName,
                    message = error.ErrorMessage
                }));
            }
            var result = await _signUpService.SignUpAsync(signUpDTO);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //SignIn Api
        [HttpPost("SignIn")]
        public async Task<ActionResult> SignIn([FromBody] SignInDTO signInDTO)
        {
            var validation = await _signInValidator.ValidateAsync(signInDTO);

            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors.Select(error => new
                {
                    field = error.PropertyName,
                    message = error.ErrorMessage
                }));
            }
            var result = await _signInService.SignInAsync(signInDTO);
            if (!result.IsSuccess)
            {
                if (result.ErrorCode == "INVALID_PASSWORD")
                {
                    return Unauthorized(result); // 401 HTTP Status
                }

                return BadRequest(result); // 400 HTTP Status
            }
            return Ok(result);
        }
    }
}