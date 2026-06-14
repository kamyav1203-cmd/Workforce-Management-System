using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs;

public class AttendanceDto
{
    public int AttendanceId { get; set; }
    public int EmpId { get; set; }
    public string? EmployeeName { get; set; }
    public DateTime CheckIn { get; set; }
    public DateTime? CheckOut { get; set; }
    public double? TotalHours { get; set; }
    public string? WorkMode { get; set; }
    public DateTime AttendanceDate { get; set; }
}

public class CheckInDto
{
    [Required]
    public int EmpId { get; set; }

    [MaxLength(20)]
    public string WorkMode { get; set; } = "WFO";
}

public class CheckOutDto
{
    [Required]
    public int AttendanceId { get; set; }
}

public class MonthlyAttendanceRequestDto
{
    [Required]
    public int EmpId { get; set; }

    [Required, Range(1, 12)]
    public int Month { get; set; }

    [Required]
    public int Year { get; set; }
}
