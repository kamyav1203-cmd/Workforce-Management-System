using Microsoft.EntityFrameworkCore;
using WMS.Domain.Entities;

namespace WMS.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(WmsDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.Roles.AnyAsync()) return;

        var roles = new[]
        {
            new Role { RoleName = "Admin", Description = "System Administrator" },
            new Role { RoleName = "Manager", Description = "Department Manager" },
            new Role { RoleName = "Employee", Description = "Regular Employee" }
        };
        context.Roles.AddRange(roles);
        await context.SaveChangesAsync();

        var departments = new[]
        {
            new Department { DepartmentName = "Human Resources", Description = "HR Department" },
            new Department { DepartmentName = "Engineering", Description = "Software Engineering" },
            new Department { DepartmentName = "Finance", Description = "Finance and Accounting" },
            new Department { DepartmentName = "Operations", Description = "Business Operations" }
        };
        context.Departments.AddRange(departments);
        await context.SaveChangesAsync();

        var adminRole = await context.Roles.FirstAsync(r => r.RoleName == "Admin");
        var managerRole = await context.Roles.FirstAsync(r => r.RoleName == "Manager");
        var employeeRole = await context.Roles.FirstAsync(r => r.RoleName == "Employee");
        var hrDept = await context.Departments.FirstAsync(d => d.DepartmentName == "Human Resources");
        var engDept = await context.Departments.FirstAsync(d => d.DepartmentName == "Engineering");

        var employees = new[]
        {
            new Employee
            {
                FirstName = "Admin", LastName = "User", Email = "admin@wms.com",
                PhoneNumber = "9876543210", Gender = "M",
                DOB = new DateTime(1990, 1, 15), DOJ = new DateTime(2020, 1, 1),
                DepartmentId = hrDept.DepartmentId, RoleId = adminRole.RoleId
            },
            new Employee
            {
                FirstName = "John", LastName = "Manager", Email = "manager@wms.com",
                PhoneNumber = "9876543211", Gender = "M",
                DOB = new DateTime(1988, 5, 20), DOJ = new DateTime(2019, 3, 15),
                DepartmentId = engDept.DepartmentId, RoleId = managerRole.RoleId
            },
            new Employee
            {
                FirstName = "Jane", LastName = "Employee", Email = "employee@wms.com",
                PhoneNumber = "9876543212", Gender = "F",
                DOB = new DateTime(1995, 8, 10), DOJ = new DateTime(2022, 6, 1),
                DepartmentId = engDept.DepartmentId, RoleId = employeeRole.RoleId
            }
        };
        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();

        var adminEmp = await context.Employees.FirstAsync(e => e.Email == "admin@wms.com");
        var managerEmp = await context.Employees.FirstAsync(e => e.Email == "manager@wms.com");
        var employeeEmp = await context.Employees.FirstAsync(e => e.Email == "employee@wms.com");

        var users = new[]
        {
            new UserLogin { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), RoleId = adminRole.RoleId, EmployeeId = adminEmp.EmployeeId },
            new UserLogin { Username = "manager", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"), RoleId = managerRole.RoleId, EmployeeId = managerEmp.EmployeeId },
            new UserLogin { Username = "employee", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Employee@123"), RoleId = employeeRole.RoleId, EmployeeId = employeeEmp.EmployeeId }
        };
        context.UserLogins.AddRange(users);

        var client = new Client { ClientName = "Acme Corp", ClientAdress = "123 Business St", ClientPhoneNumber = 5551234567, ClientLocation = "New York" };
        context.Clients.Add(client);
        await context.SaveChangesAsync();

        var project = new Project
        {
            ProjectName = "WMS Implementation",
            ClientId = client.ClientId,
            StartDate = DateTime.UtcNow.Date,
            Status = "Active"
        };
        context.Projects.Add(project);
        await context.SaveChangesAsync();

        context.Announcements.Add(new Announcement
        {
            Title = "Welcome to WMS",
            Message = "Welcome to the Workforce Management System. Please update your profile.",
            CreatedBy = adminEmp.EmployeeId,
            IsActive = true
        });

        await context.SaveChangesAsync();
    }
}
