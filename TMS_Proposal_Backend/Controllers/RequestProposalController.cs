using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.Services.RequestService;

namespace TMS_Proposal_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RequestProposalController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public RequestProposalController(
            IRequestService requestService)
        {
            _requestService = requestService;
        }

        // Create Request Endpoint
        [HttpPost("CreateNewRequest")]
        public async Task<IActionResult> CreateRequest(
            CreateRequestDTO dto)
        {
            var userIdValue = User.FindFirst("Id")?.Value;

            if (!Guid.TryParse(userIdValue, out var userId))
            {
                return Unauthorized();
            }

            var result = await _requestService.CreateRequestAsync( dto, userId);

            return result.IsSuccess? Ok(result): BadRequest(result);
        }

        // Read All Requests Endpoint
        [HttpGet("ReadAll")]
        public async Task<IActionResult> GetAllRequests()
        {
            var result =await _requestService.GetAllRequestsAsync();

            return result.IsSuccess ? Ok(result): BadRequest(result);
        }

        // Read Request By ID Endpoint
        [HttpGet("ReadById{requestId:guid}")]
        public async Task<IActionResult> GetRequestById(
            Guid requestId)
        {
            var result =await _requestService.GetRequestByIdAsync(requestId);

            if (!result.IsSuccess)
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        //Update
        [HttpPut("Update/{requestId:guid}")]
        public async Task<IActionResult> UpdateRequest(Guid requestId, [FromBody] CreateRequestDTO dto)
        {
            var userIdValue = User.FindFirst("Id")?.Value;

            if (!Guid.TryParse(userIdValue, out var updatedByUserId))
            {
                return Unauthorized();
            }
            var result = await _requestService.UpdateRequestAsync( requestId,dto, updatedByUserId);

            if (result.IsSuccess)return Ok(result);

            if (result.ErrorCode == "REQUEST_NOT_FOUND") return NotFound(result);

            if (result.ErrorCode == "SERVER_ERROR")return StatusCode(500, result);

            return BadRequest(result);
        }
        // Delete Request Endpoint
        [HttpDelete("Delete/{requestId:guid}")]
        public async Task<IActionResult> DeleteRequest(Guid requestId)
        {
            var userIdValue = User.FindFirst("Id")?.Value;

            if (!Guid.TryParse(userIdValue, out var deletedByUserId))
            {
                return Unauthorized();
            }

            var result = await _requestService.DeleteRequestAsync(requestId,deletedByUserId);

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            if (result.ErrorCode == "REQUEST_NOT_FOUND")
            {
                return NotFound(result);
            }

            if (result.ErrorCode == "SERVER_ERROR")
            {
                return StatusCode(500, result);
            }

            return BadRequest(result);
        }
    }
}
