using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAttendanceQueryRepository _attendanceRepo;

    public DashboardService(IUnitOfWork unitOfWork, IAttendanceQueryRepository attendanceRepo)
    {
        _unitOfWork = unitOfWork;
        _attendanceRepo = attendanceRepo;
    }

    public async Task<DashboardDto> GetDashboardDataAsync()
    {
        var employees = (await _unitOfWork.Employees.GetAllAsync()).ToList();
        var departments = (await _unitOfWork.Departments.GetAllAsync()).ToList();
        var projects = (await _unitOfWork.Projects.GetAllAsync()).ToList();
        var leaves = (await _unitOfWork.Leaves.GetAllAsync()).ToList();
        var attendances = await _attendanceRepo.GetAllWithDetailsAsync();

        var today = DateTime.UtcNow.Date;
        var weekStart = today.AddDays(-(int)today.DayOfWeek);

        var activeEmployeesCount = employees.Count(e => e.Status == "Active");
        var todayPresentCount = attendances.Count(a => a.AttendanceDate.Date == today);

        return new DashboardDto
        {
            TotalEmployees = employees.Count,
            ActiveEmployees = activeEmployeesCount,
            TotalDepartments = departments.Count,
            TotalProjects = projects.Count,
            ActiveProjects = projects.Count(p => p.Status == "Active"),
            PendingLeaves = leaves.Count(l => l.Status == "Pending"),
            ApprovedLeaves = leaves.Count(l => l.Status == "Approved"),
            RejectedLeaves = leaves.Count(l => l.Status == "Rejected"),
            TodayPresent = todayPresentCount,
            TodayAbsent = Math.Max(0, activeEmployeesCount - todayPresentCount),
            AttendanceChart = Enumerable.Range(0, 7).Select(i =>
            {
                var date = weekStart.AddDays(i);
                return new ChartDataDto
                {
                    Label = date.ToString("ddd"),
                    Value = attendances.Count(a => a.AttendanceDate.Date == date)
                };
            }).ToList(),
            LeaveChart = new List<ChartDataDto>
            {
                new() { Label = "Pending", Value = leaves.Count(l => l.Status == "Pending") },
                new() { Label = "Approved", Value = leaves.Count(l => l.Status == "Approved") },
                new() { Label = "Rejected", Value = leaves.Count(l => l.Status == "Rejected") }
            }
        };
    }
}
