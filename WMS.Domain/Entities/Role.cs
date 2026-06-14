using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities;

public class Role
{
    public int RoleId { get; set; }

    [Required, MaxLength(50)]
    public string RoleName { get; set; } = string.Empty;

    [MaxLength(150)]
    public string? Description { get; set; }

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    public ICollection<UserLogin> Users { get; set; } = new List<UserLogin>();
}
