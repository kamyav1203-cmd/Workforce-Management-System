using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly WmsDbContext _context;

    public UnitOfWork(WmsDbContext context)
    {
        _context = context;
        Employees = new GenericRepository<Employee>(_context);
        Departments = new GenericRepository<Department>(_context);
        Roles = new GenericRepository<Role>(_context);
        Attendances = new GenericRepository<Attendance>(_context);
        Leaves = new GenericRepository<Leave>(_context);
        Announcements = new GenericRepository<Announcement>(_context);
        Projects = new GenericRepository<Project>(_context);
        Clients = new GenericRepository<Client>(_context);
        Allocations = new GenericRepository<EmployeeProjectAllocation>(_context);
        Users = new GenericRepository<UserLogin>(_context);
        AuditLogs = new GenericRepository<AuditLog>(_context);
    }

    public IGenericRepository<Employee> Employees { get; }
    public IGenericRepository<Department> Departments { get; }
    public IGenericRepository<Role> Roles { get; }
    public IGenericRepository<Attendance> Attendances { get; }
    public IGenericRepository<Leave> Leaves { get; }
    public IGenericRepository<Announcement> Announcements { get; }
    public IGenericRepository<Project> Projects { get; }
    public IGenericRepository<Client> Clients { get; }
    public IGenericRepository<EmployeeProjectAllocation> Allocations { get; }
    public IGenericRepository<UserLogin> Users { get; }
    public IGenericRepository<AuditLog> AuditLogs { get; }

    public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}
