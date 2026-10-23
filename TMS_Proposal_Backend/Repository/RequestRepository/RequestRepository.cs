
using Microsoft.EntityFrameworkCore;
using TMS_Proposal_Backend.Data;
using TMS_Proposal_Backend.Models;

namespace TMS_Proposal_Backend.Repository.RequestRepository
{
    public class RequestRepository : IRequestRepository
    {
        private readonly AppDbContext _context;

        public RequestRepository(AppDbContext context)
        {
            _context = context;
        }

        // Create Request Proposal
        public async Task<bool> CreateRequestAsync(Request request)
        {
            _context.Requests.Add(request);

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        // Read All Requests
        public async Task<IEnumerable<Request>> GetAllRequestsAsync()
        {
            return await _context.Requests
                .AsNoTracking()
                .ToListAsync();
        }

        // Read Request By ID
        public async Task<Request?> GetRequestByIdAsync(Guid id)
        {
            return await _context.Requests
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
        }
        //Update
        public async Task<bool> UpdateRequestAsync(Request request)
        {
            _context.Requests.Update(request);

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        //Delete
        public async Task<bool> DeleteRequestAsync(Guid requestId)
        {
            var request = await _context.Requests
                .FirstOrDefaultAsync(r => r.Id == requestId);

            if (request is null)
            {
                return false;
            }

            _context.Requests.Remove(request);

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
