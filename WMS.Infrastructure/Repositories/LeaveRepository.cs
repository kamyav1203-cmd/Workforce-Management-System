using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories;

public class LeaveRepository : ILeaveQueryRepository
{
    private readonly WmsDbContext _context;

    public LeaveRepository(WmsDbContext context) => _context = context;

    public async Task<List<Leave>> GetAllWithDetailsAsync() =>
        await _context.Leaves.Include(l => l.Employee).OrderByDescending(l => l.AppliedOn).ToListAsync();

    public async Task<List<Leave>> GetByEmployeeAsync(int empId) =>
        await _context.Leaves.Include(l => l.Employee)
            .Where(l => l.EmpId == empId).OrderByDescending(l => l.AppliedOn).ToListAsync();
}
