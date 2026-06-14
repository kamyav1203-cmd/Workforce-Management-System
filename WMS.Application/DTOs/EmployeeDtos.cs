using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs;

public class EmployeeDto
{
    public int EmployeeId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public DateTime DOB { get; set; }
    public DateTime DOJ { get; set; }
    public int DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int RoleId { get; set; }
    public string? RoleName { get; set; }
    public string Status { get; set; } = "Active";
}

public class CreateEmployeeDto
{
    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required, MaxLength(80), EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required, RegularExpression("^[MFO]$")]
    public string Gender { get; set; } = "M";

    [Required]
    public DateTime DOB { get; set; }

    [Required]
    public DateTime DOJ { get; set; }

    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public int RoleId { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Active";
}

public class UpdateEmployeeDto : CreateEmployeeDto { }

public class EmployeeSearchDto
{
    public string? Name { get; set; }
    public int? EmployeeId { get; set; }
    public int? DepartmentId { get; set; }
    public string? DepartmentName { get; set; }
    public int? RoleId { get; set; }
}
