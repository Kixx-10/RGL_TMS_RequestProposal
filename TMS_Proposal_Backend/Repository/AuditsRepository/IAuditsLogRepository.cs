using TMS_Proposal_Backend.Models.Audits;

namespace TMS_Proposal_Backend.Repository.AuditsRepository
{
    public interface IAuditsLogRepoitory
    {
        Task AddAsync(AuditLogs auditLog);
    }
}