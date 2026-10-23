using TMS_Proposal_Backend.Models;

namespace TMS_Proposal_Backend.Repository.RequestRepository
{
    public interface IRequestRepository
    {
        Task<bool> CreateRequestAsync(Request request);

        Task<IEnumerable<Request>> GetAllRequestsAsync();

        Task<Request?> GetRequestByIdAsync(Guid id);

        Task<bool> UpdateRequestAsync(Request request);
        Task<bool> DeleteRequestAsync(Guid requestId);

    }
}
