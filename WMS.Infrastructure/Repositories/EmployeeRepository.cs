using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeQueryRepository
{
    private readonly WmsDbContext _context;

    public EmployeeRepository(WmsDbContext context) => _context = context;

    public async Task<List<Employee>> GetAllWithDetailsAsync() =>
        await _context.Employees.Include(e => e.Department).Include(e => e.Role).ToListAsync();

    public async Task<Employee?> GetByIdWithDetailsAsync(int id) =>
        await _context.Employees.Include(e => e.Department).Include(e => e.Role)
            .FirstOrDefaultAsync(e => e.EmployeeId == id);

    public async Task<List<Employee>> SearchAsync(string? name, int? employeeId, int? departmentId, int? roleId, string? departmentName = null)
    {
        var query = _context.Employees.Include(e => e.Department).Include(e => e.Role).AsQueryable();

        if (employeeId.HasValue)
            query = query.Where(e => e.EmployeeId == employeeId.Value);
        if (departmentId.HasValue)
            query = query.Where(e => e.DepartmentId == departmentId.Value);
        if (!string.IsNullOrWhiteSpace(departmentName))
            query = query.Where(e => e.Department != null && e.Department.DepartmentName.Contains(departmentName));
        if (roleId.HasValue)
            query = query.Where(e => e.RoleId == roleId.Value);
        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(e => (e.FirstName + " " + e.LastName).Contains(name) || e.FirstName.Contains(name) || e.LastName.Contains(name));

        return await query.ToListAsync();
    }
}
