using AutoMapper;
using Microsoft.Extensions.Logging;
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.DTOs.Common;
using TMS_Proposal_Backend.Models;
using TMS_Proposal_Backend.Repository.RequestRepository;
using TMS_Proposal_Backend.Services.AuditsLogService;

namespace TMS_Proposal_Backend.Services.RequestService
{
    public class RequestService : IRequestService
    {
        private readonly IMapper _mapper;
        private readonly IRequestRepository _requestRepository;

        private readonly ILogger<RequestService> _logger;
        private readonly IAuditsLogService _auditsLogService;

        public RequestService(
             IMapper mapper,
             IRequestRepository requestRepository,
             ILogger<RequestService> logger,
             IAuditsLogService auditsLogService
             )

        {
            _mapper = mapper;
            _requestRepository = requestRepository;
            _logger = logger;
            _auditsLogService = auditsLogService;
        }
        // Create Request
        public async Task<ServiceResponse<ResponseRequestDTO>>
            CreateRequestAsync(CreateRequestDTO dto, Guid createdByUserId)
        {
            try
            {
                var request = _mapper.Map<Request>(dto);
                request.CreatedByUserId = createdByUserId;

                var isCreated = await _requestRepository.CreateRequestAsync(request);

                if (!isCreated)
                {
                    return ServiceResponse<ResponseRequestDTO>.Failure( "CREATE_REQUEST_FAILED","Request Fail");
                }

                await _auditsLogService.LogAsync(
                createdByUserId,
                "Request Created",
                new { request.Id });
                var responseDto = _mapper.Map<ResponseRequestDTO>(request);


                return ServiceResponse<ResponseRequestDTO>.Success(responseDto,"Request Successfully created.");
            }
            catch (Exception)
            {
                return ServiceResponse<ResponseRequestDTO>.Failure("SERVER_ERROR", "Internal Server Error!");
            }
        }

        // Read All Requests
        public async Task<ServiceResponse<IEnumerable<ResponseRequestDTO>>> GetAllRequestsAsync(Guid userId, bool isAdmin)
        {
            try
            {
                var requests = await _requestRepository.GetAllRequestsAsync();

                // Admin မဟုတ်ပါက Login ဝင်ထားသော User ၏ Proposal များကိုသာ စစ်ထုတ်မည်
                if (!isAdmin)
                {
                    requests = requests.Where(r => r.CreatedByUserId == userId);
                }

                var responseDto = _mapper.Map<IEnumerable<ResponseRequestDTO>>(requests);

                return ServiceResponse<IEnumerable<ResponseRequestDTO>>.Success(responseDto, "Requests retrieved successfully.");
            }
            catch (Exception)
            {
                return ServiceResponse<IEnumerable<ResponseRequestDTO>>.Failure("SERVER_ERROR", "Internal Server Error!");
            }
        }

        // Read Request By ID
        public async Task<ServiceResponse<ResponseRequestDTO>>GetRequestByIdAsync(Guid requestId)
        {
            try
            {
                var request = await _requestRepository.GetRequestByIdAsync(requestId);

                if (request == null)
                {
                    return ServiceResponse<ResponseRequestDTO>.Failure("REQUEST_NOT_FOUND","Request not found.");
                }

                var responseDto = _mapper.Map<ResponseRequestDTO>(request);

                return ServiceResponse<ResponseRequestDTO>.Success(responseDto,"Request retrieved successfully.");
            }
            catch (Exception)
            {
                return ServiceResponse<ResponseRequestDTO>.Failure("SERVER_ERROR","Internal Server Error!");
            }
        }
        //Update
        public async Task<ServiceResponse<ResponseRequestDTO>>UpdateRequestAsync(Guid requestId, CreateRequestDTO dto, Guid updatedByUserId)
        {
            try
            {
                var request =await _requestRepository.GetRequestByIdAsync(requestId);

                if (request is null)
                {
                    return ServiceResponse<ResponseRequestDTO>.Failure("REQUEST_NOT_FOUND","Request not found.");
                }

                _mapper.Map(dto, request);

                request.UpdatedDate = DateTime.UtcNow;

                var isUpdated =await _requestRepository.UpdateRequestAsync(request);

                if (!isUpdated)
                {
                    return ServiceResponse<ResponseRequestDTO>.Failure("UPDATE_REQUEST_FAILED","Failed to update request.");
                }
                await _auditsLogService.LogAsync(
                  updatedByUserId,
                 "Request Updated",
                  new { request.Id });

                var responseDto = _mapper.Map<ResponseRequestDTO>(request);

                return ServiceResponse<ResponseRequestDTO>.Success(responseDto,"Request updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error updating request {RequestId}.",requestId);

                return ServiceResponse<ResponseRequestDTO>.Failure("SERVER_ERROR","An internal server error occurred.");
            }
        }
        //Delete Service
        public async Task<ServiceResponse<bool>> DeleteRequestAsync(Guid requestId,Guid deletedByUserId)
        {
            try
            {
                var isDeleted =await _requestRepository.DeleteRequestAsync(requestId);

                if (!isDeleted)
                {
                    return ServiceResponse<bool>.Failure("REQUEST_NOT_FOUND","Request not found.");
                }

                await _auditsLogService.LogAsync(deletedByUserId,"Request Deleted",new { requestId });

                return ServiceResponse<bool>.Success( true, "Request deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError( ex, "Error deleting request {RequestId}.", requestId);

                return ServiceResponse<bool>.Failure("SERVER_ERROR","An internal server error occurred.");
            }
        }
    }
}
