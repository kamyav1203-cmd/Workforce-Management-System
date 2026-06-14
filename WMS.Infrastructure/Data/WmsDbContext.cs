using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data;

public class WmsDbContext : DbContext
{
    public WmsDbContext(DbContextOptions<WmsDbContext> options) : base(options) { }

    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<Leave> Leaves => Set<Leave>();
    public DbSet<Announcement> Announcements => Set<Announcement>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<EmployeeProjectAllocation> EmployeeProjectAllocations => Set<EmployeeProjectAllocation>();
    public DbSet<UserLogin> UserLogins => Set<UserLogin>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Role>().HasKey(x => x.RoleId);
        modelBuilder.Entity<Department>().HasKey(x => x.DepartmentId);
        modelBuilder.Entity<Employee>().HasKey(x => x.EmployeeId);
        modelBuilder.Entity<Attendance>().HasKey(x => x.AttendanceId);
        modelBuilder.Entity<Leave>().HasKey(x => x.LeaveId);
        modelBuilder.Entity<Announcement>().HasKey(x => x.AnnouncementId);
        modelBuilder.Entity<Client>().HasKey(x => x.ClientId);
        modelBuilder.Entity<Project>().HasKey(x => x.ProjectId);
        modelBuilder.Entity<UserLogin>().HasKey(x => x.UserId);

        modelBuilder.Entity<Employee>(e =>
        {
            e.HasIndex(x => x.Email).IsUnique();
            e.Property(x => x.Status).HasDefaultValue("Active");
            e.Property(x => x.CreatedOn).HasDefaultValueSql("GETDATE()");
            e.HasOne(x => x.Department).WithMany(d => d.Employees).HasForeignKey(x => x.DepartmentId);
            e.HasOne(x => x.Role).WithMany(r => r.Employees).HasForeignKey(x => x.RoleId);
        });

        modelBuilder.Entity<Department>(e =>
        {
            e.Property(x => x.CreatedOn).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Attendance>(e =>
        {
            e.HasOne(x => x.Employee).WithMany(emp => emp.Attendances).HasForeignKey(x => x.EmpId);
        });

        modelBuilder.Entity<Leave>(e =>
        {
            e.Property(x => x.Status).HasDefaultValue("Pending");
            e.Property(x => x.AppliedOn).HasDefaultValueSql("GETDATE()");
            e.HasOne(x => x.Employee).WithMany(emp => emp.Leaves).HasForeignKey(x => x.EmpId);
        });

        modelBuilder.Entity<Announcement>(e =>
        {
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedOn).HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Project>(e =>
        {
            e.Property(x => x.Status).HasDefaultValue("Active");
            e.HasOne(x => x.Client).WithMany(c => c.Projects).HasForeignKey(x => x.ClientId);
        });

        modelBuilder.Entity<Client>(e =>
        {
            e.Property(x => x.Status).HasDefaultValue(true);
        });

        modelBuilder.Entity<EmployeeProjectAllocation>(e =>
        {
            e.HasKey(x => x.AllocationId);
            e.Property(x => x.Status).HasDefaultValue(true);
            e.Property(x => x.ApprovalStatus).HasDefaultValue("Pending");
            e.HasOne(x => x.Employee).WithMany(emp => emp.ProjectAllocations).HasForeignKey(x => x.EmpId);
            e.HasOne(x => x.Project).WithMany(p => p.Allocations).HasForeignKey(x => x.ProjectId);
        });

        modelBuilder.Entity<UserLogin>(e =>
        {
            e.HasIndex(x => x.Username).IsUnique();
            e.HasOne(x => x.Role).WithMany(r => r.Users).HasForeignKey(x => x.RoleId);
        });

        modelBuilder.Entity<AuditLog>(e =>
        {
            e.HasKey(x => x.AuditId);
            e.Property(x => x.CreatedOn).HasDefaultValueSql("GETDATE()");
        });
    }
}
