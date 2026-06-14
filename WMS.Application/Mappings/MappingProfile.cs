using AutoMapper;
using WMS.Application.DTOs;
using WMS.Domain.Entities;

namespace WMS.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Employee, EmployeeDto>()
            .ForMember(d => d.DepartmentName, o => o.MapFrom(s => s.Department != null ? s.Department.DepartmentName : null))
            .ForMember(d => d.RoleName, o => o.MapFrom(s => s.Role != null ? s.Role.RoleName : null));
        CreateMap<CreateEmployeeDto, Employee>();
        CreateMap<UpdateEmployeeDto, Employee>();

        CreateMap<Department, DepartmentDto>();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();

        CreateMap<Attendance, AttendanceDto>()
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee != null ? $"{s.Employee.FirstName} {s.Employee.LastName}" : null));

        CreateMap<Leave, LeaveDto>()
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee != null ? $"{s.Employee.FirstName} {s.Employee.LastName}" : null));
        CreateMap<ApplyLeaveDto, Leave>();

        CreateMap<Project, ProjectDto>()
            .ForMember(d => d.ClientName, o => o.MapFrom(s => s.Client != null ? s.Client.ClientName : null));
        CreateMap<CreateProjectDto, Project>();
        CreateMap<UpdateProjectDto, Project>();

        CreateMap<Client, ClientDto>();
        CreateMap<CreateClientDto, Client>();
        CreateMap<UpdateClientDto, Client>();

        CreateMap<EmployeeProjectAllocation, AllocationDto>()
            .ForMember(d => d.EmployeeName, o => o.MapFrom(s => s.Employee != null ? $"{s.Employee.FirstName} {s.Employee.LastName}" : null))
            .ForMember(d => d.ProjectName, o => o.MapFrom(s => s.Project != null ? s.Project.ProjectName : null));
        CreateMap<CreateAllocationDto, EmployeeProjectAllocation>();

        CreateMap<Announcement, AnnouncementDto>();
        CreateMap<CreateAnnouncementDto, Announcement>();
    }
}
