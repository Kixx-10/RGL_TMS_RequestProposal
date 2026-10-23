namespace TMS_Proposal_Backend.Services.AuditsLogService
{
    public interface IAuditsLogService
    {
        Task LogAsync(Guid userId, string activity, object? details);
    }
}