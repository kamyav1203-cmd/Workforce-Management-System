using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;

namespace WMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService) => _projectService = projectService;

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _projectService.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var project = await _projectService.GetByIdAsync(id);
        return project == null ? NotFound() : Ok(project);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto dto)
    {
        var created = await _projectService.CreateAsync(dto, GetUserId());
        return CreatedAtAction(nameof(GetById), new { id = created.ProjectId }, created);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProjectDto dto)
    {
        var updated = await _projectService.UpdateAsync(id, dto, GetUserId());
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _projectService.DeleteAsync(id, GetUserId());
        return result ? NoContent() : NotFound();
    }

    [HttpGet("clients")]
    public async Task<IActionResult> GetClients() => Ok(await _projectService.GetClientsAsync());

    [HttpPost("clients")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientDto dto) =>
        Ok(await _projectService.CreateClientAsync(dto, GetUserId()));

    [HttpPut("clients/{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientDto dto)
    {
        var updated = await _projectService.UpdateClientAsync(id, dto, GetUserId());
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("clients/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteClient(int id)
    {
        var result = await _projectService.DeleteClientAsync(id, GetUserId());
        return result ? NoContent() : NotFound();
    }

    [HttpGet("allocations")]
    public async Task<IActionResult> GetAllocations() => Ok(await _projectService.GetAllocationsAsync());

    [HttpPost("allocations")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> AssignEmployee([FromBody] CreateAllocationDto dto) =>
        Ok(await _projectService.AssignEmployeeAsync(dto, GetUserId()));

    [HttpPut("allocations/approve")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> ApproveRejectAllocation([FromBody] AllocationApprovalDto dto)
    {
        var result = await _projectService.ApproveRejectAllocationAsync(dto, GetUserId());
        return result == null ? BadRequest(new { message = "Cannot process allocation." }) : Ok(result);
    }

    [HttpPut("allocations/{allocationId}/cancel")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> CancelAllocation(int allocationId, [FromQuery] string updatedBy)
    {
        var result = await _projectService.CancelAllocationAsync(allocationId, updatedBy, GetUserId());
        return result ? NoContent() : NotFound();
    }
}
