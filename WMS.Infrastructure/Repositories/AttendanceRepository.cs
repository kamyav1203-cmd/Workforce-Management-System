using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories;

public class AttendanceRepository : IAttendanceQueryRepository
{
    private readonly WmsDbContext _context;

    public AttendanceRepository(WmsDbContext context) => _context = context;

    public async Task<List<Attendance>> GetMonthlyAsync(int empId, int month, int year) =>
        await _context.Attendances.Include(a => a.Employee)
            .Where(a => a.EmpId == empId && a.AttendanceDate.Month == month && a.AttendanceDate.Year == year)
            .OrderBy(a => a.AttendanceDate).ToListAsync();

    public async Task<List<Attendance>> GetAllWithDetailsAsync() =>
        await _context.Attendances.Include(a => a.Employee).OrderByDescending(a => a.AttendanceDate).ToListAsync();

    public async Task<Attendance?> GetTodayOpenAsync(int empId) =>
        await _context.Attendances.FirstOrDefaultAsync(a =>
            a.EmpId == empId && a.AttendanceDate.Date == DateTime.UtcNow.Date && a.CheckOut == null);

    public async Task<Attendance?> GetTodayRecordAsync(int empId) =>
        await _context.Attendances.FirstOrDefaultAsync(a =>
            a.EmpId == empId && a.AttendanceDate.Date == DateTime.UtcNow.Date);
}
