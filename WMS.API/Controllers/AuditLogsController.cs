using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.Services;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,Manager")]
public class AuditLogsController : ControllerBase
{
    private readonly AuditService _auditService;

    public AuditLogsController(AuditService auditService)
    {
        _auditService = auditService;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var logs = await _auditService.GetAuditLogsAsync();
        return Ok(logs);
    }
}
