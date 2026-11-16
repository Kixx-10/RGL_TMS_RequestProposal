
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.Services.TruckService;

namespace TMS_Proposal_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TruckController : ControllerBase
    {
        private readonly ITruckService _truckService;

        public TruckController(ITruckService truckService)
        {
            _truckService = truckService;
        }

        // GET: api/Truck/request/{requestId}
        [HttpGet("request/{requestId:guid}")]
        public async Task<IActionResult> GetTrucksByRequestId(Guid requestId)
        {
            var result = await _truckService.GetTrucksByRequestIdAsync(requestId);

            if (!result.IsSuccess)
                return StatusCode(500, result);

            return Ok(result);
        }

        // GET: api/Truck/{truckId}
        [HttpGet("{truckId:guid}")]
        public async Task<IActionResult> GetTruckById(Guid truckId)
        {
            var result = await _truckService.GetTruckByIdAsync(truckId);

            if (!result.IsSuccess)
            {
                if (result.ErrorCode == "TRUCK_NOT_FOUND")
                    return NotFound(result);

                return StatusCode(500, result);
            }

            return Ok(result);
        }

        // PUT: api/Truck/Update/{truckId}
        [HttpPut("Update/{truckId:guid}")]
        public async Task<IActionResult> UpdateTruck(
            Guid truckId,
            [FromBody] UpdateTruckDTO dto)
        {
            var userIdValue = User.FindFirst("Id")?.Value;

            if (!Guid.TryParse(userIdValue, out var updatedByUserId))
                return Unauthorized();

            var result = await _truckService.UpdateTruckAsync(
                truckId, dto, updatedByUserId);

            if (result.IsSuccess)
                return Ok(result);

            if (result.ErrorCode == "TRUCK_NOT_FOUND")
                return NotFound(result);

            if (result.ErrorCode == "SERVER_ERROR")
                return StatusCode(500, result);

            return BadRequest(result);
        }

        // DELETE: api/Truck/Delete/{truckId}
        [HttpDelete("Delete/{truckId:guid}")]
        public async Task<IActionResult> DeleteTruck(Guid truckId)
        {
            var userIdValue = User.FindFirst("Id")?.Value;

            if (!Guid.TryParse(userIdValue, out var deletedByUserId))
                return Unauthorized();

            var result = await _truckService.DeleteTruckAsync(
                truckId, deletedByUserId);

            if (result.IsSuccess)
                return Ok(result);

            if (result.ErrorCode == "TRUCK_NOT_FOUND")
                return NotFound(result);

            if (result.ErrorCode == "SERVER_ERROR")
                return StatusCode(500, result);

            return BadRequest(result);
        }
    }
}
