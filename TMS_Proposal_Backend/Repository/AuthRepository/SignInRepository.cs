using Microsoft.EntityFrameworkCore;
using TMS_Proposal_Backend.Data;
using TMS_Proposal_Backend.Models.Auth;

namespace TMS_Proposal_Backend.Repository.AuthRepository
{
    public class SignInRepository : ISignInRepository
    {
        private readonly AppDbContext _context;
        public SignInRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
    }
}
