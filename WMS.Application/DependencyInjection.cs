using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WMS.Application.Interfaces;
using WMS.Application.Mappings;
using WMS.Application.Services;

namespace WMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<MappingProfile>();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddScoped<AuditService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<ILeaveService, LeaveService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IAnnouncementService, AnnouncementService>();
        return services;
    }
}
