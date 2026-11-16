
using AutoMapper;
using Microsoft.Extensions.Logging;
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.DTOs.Common;
using TMS_Proposal_Backend.Models.Master;
using TMS_Proposal_Backend.Repository.TruckRepository;
using TMS_Proposal_Backend.Services.AuditsLogService;

namespace TMS_Proposal_Backend.Services.TruckService
{
    public class TruckService : ITruckService
    {
        private readonly IMapper _mapper;
        private readonly ITruckRepository _truckRepository;
        private readonly ILogger<TruckService> _logger;
        private readonly IAuditsLogService _auditsLogService;

        public TruckService(
            IMapper mapper,
            ITruckRepository truckRepository,
            ILogger<TruckService> logger,
            IAuditsLogService auditsLogService)
        {
            _mapper = mapper;
            _truckRepository = truckRepository;
            _logger = logger;
            _auditsLogService = auditsLogService;
        }

        // Read all trucks by RequestId
        public async Task<ServiceResponse<IEnumerable<ResponseTruckDTO>>>
            GetTrucksByRequestIdAsync(Guid requestId)
        {
            try
            {
                var trucks = await _truckRepository
                    .GetTrucksByRequestIdAsync(requestId);

                var responseDto =
                    _mapper.Map<IEnumerable<ResponseTruckDTO>>(trucks);

                return ServiceResponse<IEnumerable<ResponseTruckDTO>>
                    .Success(responseDto, "Trucks retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error getting trucks for Request {RequestId}.", requestId);

                return ServiceResponse<IEnumerable<ResponseTruckDTO>>
                    .Failure("SERVER_ERROR", "Internal Server Error!");
            }
        }

        // Read one truck by TruckId
        public async Task<ServiceResponse<ResponseTruckDTO>>
            GetTruckByIdAsync(Guid truckId)
        {
            try
            {
                var truck = await _truckRepository
                    .GetTruckByIdAsync(truckId);

                if (truck == null)
                {
                    return ServiceResponse<ResponseTruckDTO>
                        .Failure("TRUCK_NOT_FOUND", "Truck not found.");
                }

                var responseDto = _mapper.Map<ResponseTruckDTO>(truck);

                return ServiceResponse<ResponseTruckDTO>
                    .Success(responseDto, "Truck retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error getting truck {TruckId}.", truckId);

                return ServiceResponse<ResponseTruckDTO>
                    .Failure("SERVER_ERROR", "Internal Server Error!");
            }
        }

        // Update one truck
        public async Task<ServiceResponse<bool>>
            UpdateTruckAsync(Guid truckId, UpdateTruckDTO dto, Guid updatedByUserId)
        {
            try
            {
                var existingTruck = await _truckRepository
                    .GetTruckByIdAsync(truckId);

                if (existingTruck == null)
                {
                    return ServiceResponse<bool>
                        .Failure("TRUCK_NOT_FOUND", "Truck not found.");
                }

                existingTruck.TruckNo = dto.TruckNo;
                existingTruck.JobType = dto.JobType;
                existingTruck.ETA = dto.ETA;
                existingTruck.UpdatedDate = DateTime.UtcNow;

                var isUpdated = await _truckRepository
                    .UpdateTruckAsync(existingTruck);

                if (!isUpdated)
                {
                    return ServiceResponse<bool>
                        .Failure("UPDATE_TRUCK_FAILED", "Failed to update truck.");
                }

                await _auditsLogService.LogAsync(
                    updatedByUserId,
                    "Truck Updated",
                    new { existingTruck.TruckId, existingTruck.TruckNo });

                return ServiceResponse<bool>
                    .Success(true, "Truck updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error updating truck {TruckId}.", truckId);

                return ServiceResponse<bool>
                    .Failure("SERVER_ERROR", "Internal Server Error!");
            }
        }

        // Delete one truck
        public async Task<ServiceResponse<bool>>
            DeleteTruckAsync(Guid truckId, Guid deletedByUserId)
        {
            try
            {
                var isDeleted = await _truckRepository
                    .DeleteTruckAsync(truckId);

                if (!isDeleted)
                {
                    return ServiceResponse<bool>
                        .Failure("TRUCK_NOT_FOUND", "Truck not found.");
                }

                await _auditsLogService.LogAsync(
                    deletedByUserId,
                    "Truck Deleted",
                    new { truckId });

                return ServiceResponse<bool>
                    .Success(true, "Truck deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error deleting truck {TruckId}.", truckId);

                return ServiceResponse<bool>
                    .Failure("SERVER_ERROR", "Internal Server Error!");
            }
        }
    }
}
