using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.DTOs.Common;

namespace TMS_Proposal_Backend.Services.RequestService
{
    public interface IRequestService
    {
        // Create
        Task<ServiceResponse<ResponseRequestDTO>> CreateRequestAsync(
            CreateRequestDTO dto,
            Guid createdByUserId);

        // Read all requests
        Task<ServiceResponse<IEnumerable<ResponseRequestDTO>>> GetAllRequestsAsync(Guid userId, bool isAdmin);

        // Read request by ID
        Task<ServiceResponse<ResponseRequestDTO>> GetRequestByIdAsync(Guid requestId);

        Task<ServiceResponse<ResponseRequestDTO>> UpdateRequestAsync( Guid requestId,CreateRequestDTO dto, Guid updatedByUserId);
        Task<ServiceResponse<bool>> DeleteRequestAsync(Guid requestId,Guid deletedByUserId);


    }
}
