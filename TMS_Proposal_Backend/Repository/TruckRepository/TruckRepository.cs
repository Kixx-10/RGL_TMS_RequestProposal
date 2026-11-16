
using Microsoft.EntityFrameworkCore;
using TMS_Proposal_Backend.Data;
using TMS_Proposal_Backend.Models.Master;

namespace TMS_Proposal_Backend.Repository.TruckRepository
{
    public class TruckRepository : ITruckRepository
    {
        private readonly AppDbContext _context;

        public TruckRepository(AppDbContext context)
        {
            _context = context;
        }

        // Read all trucks by RequestId
        public async Task<IEnumerable<Truck>> GetTrucksByRequestIdAsync(Guid requestId)
        {
            return await _context.Trucks
                .Where(t => t.RequestId == requestId)
                .AsNoTracking()
                .ToListAsync();
        }

        // Read one truck by TruckId
        public async Task<Truck?> GetTruckByIdAsync(Guid truckId)
        {
            return await _context.Trucks
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.TruckId == truckId);
        }

        // Update one truck
        public async Task<bool> UpdateTruckAsync(Truck truck)
        {
            var existingTruck = await _context.Trucks
                .FirstOrDefaultAsync(t => t.TruckId == truck.TruckId);

            if (existingTruck == null)
            {
                return false;
            }

            existingTruck.TruckNo = truck.TruckNo;
            existingTruck.JobType = truck.JobType;
            existingTruck.ETA = truck.ETA;
            existingTruck.UpdatedDate = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }

        // Delete one truck
        public async Task<bool> DeleteTruckAsync(Guid truckId)
        {
            var truck = await _context.Trucks
                .FirstOrDefaultAsync(t => t.TruckId == truckId);

            if (truck == null)
            {
                return false;
            }

            _context.Trucks.Remove(truck);

            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
    }
}
