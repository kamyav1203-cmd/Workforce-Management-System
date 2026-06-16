using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface IAttendanceQueryRepository
{
    Task<List<Attendance>> GetMonthlyAsync(int empId, int month, int year);
    Task<List<Attendance>> GetAllWithDetailsAsync();
    Task<Attendance?> GetTodayOpenAsync(int empId);
    Task<Attendance?> GetTodayRecordAsync(int empId);
}
