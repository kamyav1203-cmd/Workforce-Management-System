using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities;

public class Attendance
{
    public int AttendanceId { get; set; }
    public int EmpId { get; set; }

    [Required]
    public DateTime CheckIn { get; set; }

    public DateTime? CheckOut { get; set; }
    public double? TotalHours { get; set; }

    [MaxLength(20)]
    public string? WorkMode { get; set; }

    [Required]
    public DateTime AttendanceDate { get; set; }

    public Employee? Employee { get; set; }
}
