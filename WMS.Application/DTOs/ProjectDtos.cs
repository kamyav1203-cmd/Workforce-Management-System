using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs;

public class ProjectDto
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public int? ClientId { get; set; }
    public string? ClientName { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = "Active";
}

public class CreateProjectDto
{
    [Required, MaxLength(100)]
    public string ProjectName { get; set; } = string.Empty;

    public int? ClientId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Active";
}

public class UpdateProjectDto : CreateProjectDto { }

public class UpdateClientDto : CreateClientDto { }

public class ClientDto
{
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string? ClientAdress { get; set; }
    public decimal? ClientPhoneNumber { get; set; }
    public string? ClientLocation { get; set; }
    public bool Status { get; set; }
}

public class CreateClientDto
{
    [Required, MaxLength(100)]
    public string ClientName { get; set; } = string.Empty;

    public string? ClientAdress { get; set; }
    public decimal? ClientPhoneNumber { get; set; }

    [MaxLength(20)]
    public string? ClientLocation { get; set; }

    public bool Status { get; set; } = true;
}

public class AllocationDto
{
    public int AllocationId { get; set; }
    public int EmpId { get; set; }
    public string? EmployeeName { get; set; }
    public int ProjectId { get; set; }
    public string? ProjectName { get; set; }
    public DateTime AssignedOn { get; set; }
    public bool Status { get; set; }
    public string ApprovalStatus { get; set; } = "Pending";
}

public class AllocationApprovalDto
{
    [Required]
    public int AllocationId { get; set; }

    [Required, RegularExpression("^(Approved|Rejected)$")]
    public string ApprovalStatus { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string UpdatedBy { get; set; } = string.Empty;
}

public class CreateAllocationDto
{
    [Required]
    public int EmpId { get; set; }

    [Required]
    public int ProjectId { get; set; }

    [Required]
    public DateTime AssignedOn { get; set; }

    [Required, MaxLength(50)]
    public string CreatedBY { get; set; } = string.Empty;
}

public class AnnouncementDto
{
    public int AnnouncementId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int CreatedBy { get; set; }
    public DateTime CreatedOn { get; set; }
    public bool IsActive { get; set; }
}

public class CreateAnnouncementDto
{
    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    [Required]
    public int CreatedBy { get; set; }
}

public class UpdateAnnouncementDto
{
    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public class DashboardDto
{
    public int TotalEmployees { get; set; }
    public int ActiveEmployees { get; set; }
    public int TotalDepartments { get; set; }
    public int TotalProjects { get; set; }
    public int ActiveProjects { get; set; }
    public int PendingLeaves { get; set; }
    public int ApprovedLeaves { get; set; }
    public int RejectedLeaves { get; set; }
    public int TodayPresent { get; set; }
    public int TodayAbsent { get; set; }
    public List<ChartDataDto> AttendanceChart { get; set; } = new();
    public List<ChartDataDto> LeaveChart { get; set; } = new();
}

public class ChartDataDto
{
    public string Label { get; set; } = string.Empty;
    public int Value { get; set; }
}
