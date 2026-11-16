
using TMS_Proposal_Backend.Models.Master;

namespace TMS_Proposal_Backend.Repository.TruckRepository
{
    public interface ITruckRepository
    {
        Task<IEnumerable<Truck>> GetTrucksByRequestIdAsync(Guid requestId);

        Task<Truck?> GetTruckByIdAsync(Guid truckId);

        Task<bool> UpdateTruckAsync(Truck truck);

        Task<bool> DeleteTruckAsync(Guid truckId);
    }
}
