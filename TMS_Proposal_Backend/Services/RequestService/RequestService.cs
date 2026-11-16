using AutoMapper;
using Microsoft.EntityFrameworkCore; 
using Microsoft.Extensions.Logging;
using TMS_Proposal_Backend.Data; 
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.DTOs.Common;
using TMS_Proposal_Backend.Models;
using TMS_Proposal_Backend.Models.Master;
using TMS_Proposal_Backend.Repository.RequestRepository;
using TMS_Proposal_Backend.Services.AuditsLogService;

namespace TMS_Proposal_Backend.Services.RequestService
{
    public class RequestService : IRequestService
    {
        private readonly IMapper _mapper;
        private readonly IRequestRepository _requestRepository;
        private readonly AppDbContext _context; 
        private readonly ILogger<RequestService> _logger;
        private readonly IAuditsLogService _auditsLogService;

        public RequestService(
             IMapper mapper,
             IRequestRepository requestRepository,
             AppDbContext context,
             ILogger<RequestService> logger,
             IAuditsLogService auditsLogService
             )
        {
            _mapper = mapper;
            _requestRepository = requestRepository;
            _context = context;
            _logger = logger;
            _auditsLogService = auditsLogService;
        }

        // Create Request
        public async Task<ServiceResponse<ResponseRequestDTO>> CreateRequestAsync(CreateRequestDTO dto, Guid createdByUserId)
        {
            try
            {
                var request = _mapper.Map<Request>(dto);
                request.CreatedByUserId = createdByUserId;

                var createdRequest = await _requestRepository.CreateRequestAsync(request);

                await _auditsLogService.LogAsync( createdByUserId,"Request Created", new { createdRequest.Id });

                var responseDto = _mapper.Map<ResponseRequestDTO>(createdRequest);

                return ServiceResponse<ResponseRequestDTO>.Success(responseDto, "Request Successfully created.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating request.");
                return ServiceResponse<ResponseRequestDTO>.Failure("SERVER_ERROR", "Internal Server Error!");
            }
        }

        // Read All Requests
        public async Task<ServiceResponse<IEnumerable<ResponseRequestDTO>>> GetAllRequestsAsync(Guid userId, bool isAdmin)
        {
            try
            {
                var requests = await _requestRepository.GetAllRequestsAsync();

                if (!isAdmin)
                {
                    requests = requests.Where(r => r.CreatedByUserId == userId);
                }

                var responseDto = _mapper.Map<IEnumerable<ResponseRequestDTO>>(requests);

                return ServiceResponse<IEnumerable<ResponseRequestDTO>>.Success(responseDto, "Requests retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all requests.");
                return ServiceResponse<IEnumerable<ResponseRequestDTO>>.Failure("SERVER_ERROR", "Internal Server Error!");
            }
        }

        // Read Request By ID
        public async Task<ServiceResponse<ResponseRequestDTO>> GetRequestByIdAsync(Guid requestId)
        {
            try
            {
                var request = await _requestRepository.GetRequestByIdAsync(requestId);

                if (request == null)
                {
                    return ServiceResponse<ResponseRequestDTO>.Failure("REQUEST_NOT_FOUND", "Request not found.");
                }

                var responseDto = _mapper.Map<ResponseRequestDTO>(request);

                return ServiceResponse<ResponseRequestDTO>.Success(responseDto, "Request retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting request by ID.");
                return ServiceResponse<ResponseRequestDTO>.Failure("SERVER_ERROR", "Internal Server Error!");
            }
        }


        // Update Request
        public async Task<ServiceResponse<ResponseRequestDTO>> UpdateRequestAsync(Guid requestId,UpdateRequestDTO dto,Guid updatedByUserId)
        {
            try
            {
                var request = await _context.Requests
                    .Include(r => r.Trucks)
                    .Include(r => r.Attachments)
                    .FirstOrDefaultAsync(r => r.Id == requestId);

                if (request is null)
                {
                    return ServiceResponse<ResponseRequestDTO>.Failure("REQUEST_NOT_FOUND", "Request not found.");
                }

                if (dto.CustomerName is not null)
                {
                    request.CustomerName = dto.CustomerName;
                }

                if (dto.JobDate.HasValue)
                {
                    request.JobDate = dto.JobDate.Value;
                }

                request.UpdatedDate = DateTime.UtcNow;

               
                if (dto.Trucks is not null)
                {
                    var oldTrucks = request.Trucks.ToList();
                    _context.Trucks.RemoveRange(oldTrucks);
                    request.Trucks.Clear();

                    var newTrucks = _mapper.Map<List<Truck>>(dto.Trucks);

                    foreach (var truck in newTrucks)
                    {
                        truck.RequestId = request.Id;
                        request.Trucks.Add(truck);
                    }

                    _context.Trucks.AddRange(newTrucks);
                }

               
                if (dto.Attachments is not null)
                {
                    var oldAttachments = request.Attachments.ToList();
                    _context.Set<RequestAttachment>().RemoveRange(oldAttachments);
                    request.Attachments.Clear();

                    var validAttachments = dto.Attachments
                        .Where(a => !string.IsNullOrWhiteSpace(a.FileUrl))
                        .ToList();

                    var newAttachments =_mapper.Map<List<RequestAttachment>>(validAttachments);

                    foreach (var attachment in newAttachments)
                    {
                        attachment.RequestId = request.Id;
                        request.Attachments.Add(attachment);
                    }

                    _context.Set<RequestAttachment>().AddRange(newAttachments);
                }

                var isUpdated = await _requestRepository.UpdateRequestAsync(request);

                if (!isUpdated)
                {
                    return ServiceResponse<ResponseRequestDTO>.Failure( "UPDATE_REQUEST_FAILED","Failed to update request.");
                }

                await _auditsLogService.LogAsync( updatedByUserId, "Request Updated", new { request.Id });

                var responseDto = _mapper.Map<ResponseRequestDTO>(request);

                return ServiceResponse<ResponseRequestDTO>.Success( responseDto, "Request updated successfully.");
            }
            catch (Exception ex)
            {
                var errorMsg = ex.InnerException?.Message ?? ex.Message;

                _logger.LogError( ex, "Error updating request {RequestId}: {Error}",requestId,errorMsg);

                return ServiceResponse<ResponseRequestDTO>.Failure("SERVER_ERROR", $"Error: {errorMsg}");
            }
        }

        // Delete Service
        public async Task<ServiceResponse<bool>> DeleteRequestAsync(Guid requestId, Guid deletedByUserId)
        {
            try
            {
                var isDeleted = await _requestRepository.DeleteRequestAsync(requestId);

                if (!isDeleted)
                {
                    return ServiceResponse<bool>.Failure("REQUEST_NOT_FOUND", "Request not found.");
                }

                await _auditsLogService.LogAsync(deletedByUserId, "Request Deleted", new { requestId });

                return ServiceResponse<bool>.Success(true, "Request deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting request {RequestId}.", requestId);
                return ServiceResponse<bool>.Failure("SERVER_ERROR", "An internal server error occurred.");
            }
        }
    }
}