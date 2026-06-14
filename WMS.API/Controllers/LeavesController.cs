using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LeavesController : ControllerBase
{
    private readonly ILeaveService _leaveService;

    public LeavesController(ILeaveService leaveService) => _leaveService = leaveService;

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll() => Ok(await _leaveService.GetAllAsync());

    [HttpGet("employee/{empId}")]
    public async Task<IActionResult> GetByEmployee(int empId)
    {
        var userRole = User.FindFirstValue(ClaimTypes.Role);
        if (userRole == "Employee")
        {
            var empIdClaim = User.FindFirst("EmployeeId")?.Value;
            if (empIdClaim == null || int.Parse(empIdClaim) != empId)
            {
                return Forbid();
            }
        }
        return Ok(await _leaveService.GetByEmployeeAsync(empId));
    }

    [HttpPost("apply")]
    public async Task<IActionResult> Apply([FromBody] ApplyLeaveDto dto)
    {
        try
        {
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole == "Employee")
            {
                var empIdClaim = User.FindFirst("EmployeeId")?.Value;
                if (empIdClaim == null || int.Parse(empIdClaim) != dto.EmpId)
                {
                    return Forbid();
                }
            }

            var result = await _leaveService.ApplyAsync(dto, GetUserId());
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{leaveId}")]
    public async Task<IActionResult> Cancel(int leaveId)
    {
        var result = await _leaveService.CancelAsync(leaveId, GetUserId());
        return result ? NoContent() : BadRequest(new { message = "Cannot cancel this leave." });
    }

    [HttpPut("approve")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> ApproveReject([FromBody] LeaveApprovalDto dto)
    {
        try
        {
            var employeeIdClaim = User.FindFirst("EmployeeId")?.Value;
            if (employeeIdClaim != null)
            {
                dto.ApprovedBy = int.Parse(employeeIdClaim);
            }
            var result = await _leaveService.ApproveRejectAsync(dto, GetUserId());
            return result == null ? BadRequest(new { message = "Cannot process this leave request." }) : Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
