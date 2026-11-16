
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.DTOs.Common;

namespace TMS_Proposal_Backend.Services.TruckService
{
    public interface ITruckService
    {
        // Read all trucks by RequestId
        Task<ServiceResponse<IEnumerable<ResponseTruckDTO>>>
            GetTrucksByRequestIdAsync(Guid requestId);

        // Read one truck by TruckId
        Task<ServiceResponse<ResponseTruckDTO>>
            GetTruckByIdAsync(Guid truckId);

        // Update one truck
        Task<ServiceResponse<bool>>
            UpdateTruckAsync(Guid truckId, UpdateTruckDTO dto, Guid updatedByUserId);

        // Delete one truck
        Task<ServiceResponse<bool>>
            DeleteTruckAsync(Guid truckId, Guid deletedByUserId);
    }
}
