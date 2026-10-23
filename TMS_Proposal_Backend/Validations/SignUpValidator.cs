using FluentValidation;
using Microsoft.Identity.Client;
using TMS_Proposal_Backend.DTOs;

namespace TMS_Proposal_Backend.Validations
{
    public class SignUpValidator : AbstractValidator<SignUpDTO>
    {
        public SignUpValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(50).WithMessage("Email address not exceeds 50 characters")
                .EmailAddress().WithMessage("Invalid Email");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password field required")
                .MinimumLength(3).WithMessage("Password must be at least 3")
                .MaximumLength(255).WithMessage("Psswords not exceed 255 characters");
            RuleFor(x => x.IsAdmin)
                .NotEmpty().WithMessage("Verify Admin or no");

        }
    }
}