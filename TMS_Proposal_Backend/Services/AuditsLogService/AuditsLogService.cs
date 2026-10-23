using TMS_Proposal_Backend.Models.Audits;
using TMS_Proposal_Backend.Repository.AuditsRepository;

namespace TMS_Proposal_Backend.Services.AuditsLogService
{
    public class AuditsLogService : IAuditsLogService
    {
        private readonly IAuditsLogRepoitory _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AuditsLogService(IAuditsLogRepoitory repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogAsync(Guid userId, string activity, object? details)
        {
            var log = new AuditLogs
            {
                UserId = userId,
                LogTime = DateTime.UtcNow,
                LogIPAddr = _httpContextAccessor.HttpContext?
               .Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0",
                Activity = activity,
                Inputted = details is null
               ? null
               : System.Text.Json.JsonSerializer.Serialize(details)
            };
            await _repository.AddAsync(log);
        }
    }
}