using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var users = await _unitOfWork.Users.FindAsync(u => u.Username == request.Username);
        var user = users.FirstOrDefault();
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return null;

        var roles = await _unitOfWork.Roles.GetByIdAsync(user.RoleId);
        if (roles == null) return null;

        user.LastLogin = DateTime.UtcNow;
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        var token = GenerateJwtToken(user, roles.RoleName);

        return new LoginResponseDto
        {
            Token = token,
            Username = user.Username,
            Role = roles.RoleName,
            UserId = user.UserId,
            EmployeeId = user.EmployeeId
        };
    }

    private string GenerateJwtToken(Domain.Entities.UserLogin user, string roleName)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, roleName),
            new Claim("EmployeeId", user.EmployeeId?.ToString() ?? "0")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
