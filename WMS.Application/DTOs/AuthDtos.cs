using System.ComponentModel.DataAnnotations;

namespace WMS.Application.DTOs;

public class LoginRequestDto
{
    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int? EmployeeId { get; set; }
}
