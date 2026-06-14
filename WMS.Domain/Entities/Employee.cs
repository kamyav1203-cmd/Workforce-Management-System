using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities;

public class Employee
{
    public int EmployeeId { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(15)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required, MaxLength(1)]
    public string Gender { get; set; } = "M";

    [Required]
    public DateTime DOB { get; set; }

    [Required]
    public DateTime DOJ { get; set; }

    public int DepartmentId { get; set; }
    public int RoleId { get; set; }

    [MaxLength(20)]
    public string Status { get; set; } = "Active";

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedOn { get; set; }

    public Department? Department { get; set; }
    public Role? Role { get; set; }
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<Leave> Leaves { get; set; } = new List<Leave>();
    public ICollection<EmployeeProjectAllocation> ProjectAllocations { get; set; } = new List<EmployeeProjectAllocation>();
}
