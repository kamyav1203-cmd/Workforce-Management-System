using WMS.Domain.Entities;

namespace WMS.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IGenericRepository<Employee> Employees { get; }
    IGenericRepository<Department> Departments { get; }
    IGenericRepository<Role> Roles { get; }
    IGenericRepository<Attendance> Attendances { get; }
    IGenericRepository<Leave> Leaves { get; }
    IGenericRepository<Announcement> Announcements { get; }
    IGenericRepository<Project> Projects { get; }
    IGenericRepository<Client> Clients { get; }
    IGenericRepository<EmployeeProjectAllocation> Allocations { get; }
    IGenericRepository<UserLogin> Users { get; }
    IGenericRepository<AuditLog> AuditLogs { get; }
    Task<int> SaveChangesAsync();
}
