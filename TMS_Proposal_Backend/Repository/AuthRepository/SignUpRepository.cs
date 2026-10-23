using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TMS_Proposal_Backend.Data;
using TMS_Proposal_Backend.Models.Auth;

namespace TMS_Proposal_Backend.Repository.AuthRepository
{
    public class SignUpRepository : ISignUpRepository
    {
        private readonly AppDbContext _context;
        public SignUpRepository(AppDbContext context) 
        {
            _context = context;
        }

        public async Task<bool> IsExixtedEmailAsync(string email)
        {
         return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> AddUserAsync(User user)
        {
            _context.Users.Add(user);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
