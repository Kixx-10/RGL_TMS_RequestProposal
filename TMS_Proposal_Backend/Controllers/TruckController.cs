using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TMS_Proposal_Backend.DTOs;
using TMS_Proposal_Backend.DTOs.Common;
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
            var isAdminValue = User.FindFirst("IsAdmin")?.Value;

            if (!Guid.TryParse(userIdValue, out var updatedByUserId))
                return Unauthorized();

            bool isAdmin = bool.TryParse(isAdminValue, out var adminResult) && adminResult;

            // လက်ရှိကားအချက်အလက်ကို စစ်ဆေးခြင်း
            var existingTruck = await _truckService.GetTruckByIdAsync(truckId);
            if (!existingTruck.IsSuccess || existingTruck.Data == null)
                return NotFound(existingTruck);

            // Arrived ဖြစ်သွားပြီးသော ကားကို Customer ဘက်မှ ပြန်ပြင်ခွင့်မပြုပါ
            if (!isAdmin && existingTruck.Data.IsArrived)
            {
                return BadRequest(ServiceResponse<bool>.Failure(
                    "TRUCK_ALREADY_ARRIVED",
                    "Arrived ဖြစ်ပြီးသားကားကို Customer ဘက်မှ ပြန်လည်ပြင်ဆင်၍ မရနိုင်ပါ။"));
            }

            // Customer သည် IsArrived status ကို ပြောင်းလဲခွင့်မရှိပါ
            if (!isAdmin)
            {
                dto.IsArrived = existingTruck.Data.IsArrived;
            }

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
            var isAdminValue = User.FindFirst("IsAdmin")?.Value;

            if (!Guid.TryParse(userIdValue, out var deletedByUserId))
                return Unauthorized();

            bool isAdmin = bool.TryParse(isAdminValue, out var adminResult) && adminResult;

            var existingTruck = await _truckService.GetTruckByIdAsync(truckId);
            if (!existingTruck.IsSuccess || existingTruck.Data == null)
                return NotFound(existingTruck);

            // Arrived ဖြစ်သွားပြီးသော ကားကို Customer ဘက်မှ ဖျက်ခွင့်မပြုပါ
            if (!isAdmin && existingTruck.Data.IsArrived)
            {
                return BadRequest(ServiceResponse<bool>.Failure(
                    "TRUCK_ALREADY_ARRIVED",
                    "Arrived ဖြစ်ပြီးသားကားကို Customer ဘက်မှ ဖျက်၍ မရနိုင်ပါ။"));
            }

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