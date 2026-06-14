using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;

    public AttendanceController(IAttendanceService attendanceService) => _attendanceService = attendanceService;

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _attendanceService.GetAllAsync());

    [HttpPost("checkin")]
    public async Task<IActionResult> CheckIn([FromBody] CheckInDto dto)
    {
        try
        {
            var result = await _attendanceService.CheckInAsync(dto, GetUserId());
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> CheckOut([FromBody] CheckOutDto dto)
    {
        var result = await _attendanceService.CheckOutAsync(dto, GetUserId());
        return result == null ? BadRequest(new { message = "Invalid checkout request." }) : Ok(result);
    }

    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthly([FromQuery] MonthlyAttendanceRequestDto request) =>
        Ok(await _attendanceService.GetMonthlyAsync(request));

    [HttpGet("timesheet")]
    public async Task<IActionResult> GetTimesheetReport([FromQuery] int empId, [FromQuery] int month, [FromQuery] int year)
    {
        var report = await _attendanceService.GenerateTimesheetReportAsync(empId, month, year);
        return File(report, "application/pdf", $"Timesheet_{empId}_{month}_{year}.pdf");
    }
}
