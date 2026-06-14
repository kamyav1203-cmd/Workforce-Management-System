using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs;

public class DepartmentDto
{
    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedOn { get; set; }
}

public class CreateDepartmentDto
{
    [Required, MaxLength(100)]
    public string DepartmentName { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Description { get; set; }
}

public class UpdateDepartmentDto : CreateDepartmentDto { }
