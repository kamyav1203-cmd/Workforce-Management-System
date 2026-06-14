using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Domain.Interfaces;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RolesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public RolesController(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var roles = await _unitOfWork.Roles.GetAllAsync();
        return Ok(roles.Select(r => new { r.RoleId, r.RoleName, r.Description }));
    }
}
