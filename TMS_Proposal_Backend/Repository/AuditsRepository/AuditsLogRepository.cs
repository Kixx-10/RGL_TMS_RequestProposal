using TMS_Proposal_Backend.Data;
using TMS_Proposal_Backend.Models.Audits;

namespace TMS_Proposal_Backend.Repository.AuditsRepository
{
    public class AuditsLogRepository : IAuditsLogRepoitory
    {
        private readonly AppDbContext _context;
        public AuditsLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLogs auditLog)
        {
            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}