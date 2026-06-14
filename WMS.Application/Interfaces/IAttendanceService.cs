using WMS.Application.DTOs;

namespace WMS.Application.Interfaces;

public interface IAttendanceService
{
    Task<AttendanceDto> CheckInAsync(CheckInDto dto, int userId);
    Task<AttendanceDto?> CheckOutAsync(CheckOutDto dto, int userId);
    Task<IEnumerable<AttendanceDto>> GetMonthlyAsync(MonthlyAttendanceRequestDto request);
    Task<IEnumerable<AttendanceDto>> GetAllAsync();
    Task<byte[]> GenerateTimesheetReportAsync(int empId, int month, int year);
}
