using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IProjectQueryRepository _queryRepo;
    private readonly IMapper _mapper;
    private readonly AuditService _audit;

    public ProjectService(IUnitOfWork unitOfWork, IProjectQueryRepository queryRepo, IMapper mapper, AuditService audit)
    {
        _unitOfWork = unitOfWork;
        _queryRepo = queryRepo;
        _mapper = mapper;
        _audit = audit;
    }

    public async Task<IEnumerable<ProjectDto>> GetAllAsync()
    {
        var projects = await _queryRepo.GetAllWithClientAsync();
        return _mapper.Map<IEnumerable<ProjectDto>>(projects);
    }

    public async Task<ProjectDto?> GetByIdAsync(int id)
    {
        var project = await _queryRepo.GetByIdWithClientAsync(id);
        return project == null ? null : _mapper.Map<ProjectDto>(project);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto dto, int userId)
    {
        var project = _mapper.Map<Project>(dto);
        await _unitOfWork.Projects.AddAsync(project);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Project", project.ProjectId, "Insert", userId);
        var created = await _queryRepo.GetByIdWithClientAsync(project.ProjectId);
        return _mapper.Map<ProjectDto>(created!);
    }

    public async Task<ProjectDto?> UpdateAsync(int id, UpdateProjectDto dto, int userId)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        if (project == null) return null;
        _mapper.Map(dto, project);
        await _unitOfWork.Projects.UpdateAsync(project);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Project", id, "Update", userId);
        var updated = await _queryRepo.GetByIdWithClientAsync(id);
        return _mapper.Map<ProjectDto>(updated!);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        if (project == null) return false;
        project.Status = "Completed";
        await _unitOfWork.Projects.UpdateAsync(project);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Project", id, "Delete", userId);
        return true;
    }

    public async Task<IEnumerable<ClientDto>> GetClientsAsync()
    {
        var clients = await _unitOfWork.Clients.GetAllAsync();
        return _mapper.Map<IEnumerable<ClientDto>>(clients);
    }

    public async Task<ClientDto> CreateClientAsync(CreateClientDto dto, int userId)
    {
        var client = _mapper.Map<Client>(dto);
        await _unitOfWork.Clients.AddAsync(client);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Client", client.ClientId, "Insert", userId);
        return _mapper.Map<ClientDto>(client);
    }

    public async Task<ClientDto?> UpdateClientAsync(int id, UpdateClientDto dto, int userId)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(id);
        if (client == null) return null;
        _mapper.Map(dto, client);
        await _unitOfWork.Clients.UpdateAsync(client);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Client", id, "Update", userId);
        return _mapper.Map<ClientDto>(client);
    }

    public async Task<bool> DeleteClientAsync(int id, int userId)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(id);
        if (client == null) return false;
        client.Status = false;
        await _unitOfWork.Clients.UpdateAsync(client);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Client", id, "Delete", userId);
        return true;
    }

    public async Task<IEnumerable<AllocationDto>> GetAllocationsAsync()
    {
        var allocations = await _queryRepo.GetAllocationsAsync();
        return _mapper.Map<IEnumerable<AllocationDto>>(allocations);
    }

    public async Task<AllocationDto> AssignEmployeeAsync(CreateAllocationDto dto, int userId)
    {
        var allocation = _mapper.Map<EmployeeProjectAllocation>(dto);
        allocation.CreateDate = DateTime.UtcNow;
        allocation.Status = true;
        allocation.ApprovalStatus = "Pending";
        await _unitOfWork.Allocations.AddAsync(allocation);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("EmployeeProjectAllocation", allocation.AllocationId, "Insert", userId);

        var allocations = await _queryRepo.GetAllocationsAsync();
        var created = allocations.First(a => a.AllocationId == allocation.AllocationId);
        return _mapper.Map<AllocationDto>(created);
    }

    public async Task<AllocationDto?> ApproveRejectAllocationAsync(AllocationApprovalDto dto, int userId)
    {
        var allocation = await _unitOfWork.Allocations.GetByIdAsync(dto.AllocationId);
        if (allocation == null || allocation.ApprovalStatus != "Pending") return null;
        allocation.ApprovalStatus = dto.ApprovalStatus;
        allocation.UpdatedBy = dto.UpdatedBy;
        allocation.UpdatedDate = DateTime.UtcNow.Date;
        if (dto.ApprovalStatus == "Rejected") allocation.Status = false;
        await _unitOfWork.Allocations.UpdateAsync(allocation);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("EmployeeProjectAllocation", dto.AllocationId, "Update", userId);
        var allocations = await _queryRepo.GetAllocationsAsync();
        var updated = allocations.FirstOrDefault(a => a.AllocationId == dto.AllocationId);
        return updated == null ? null : _mapper.Map<AllocationDto>(updated);
    }

    public async Task<bool> CancelAllocationAsync(int allocationId, string updatedBy, int userId)
    {
        var allocation = await _unitOfWork.Allocations.GetByIdAsync(allocationId);
        if (allocation == null) return false;
        allocation.Status = false;
        allocation.UpdatedBy = updatedBy;
        allocation.UpdatedDate = DateTime.UtcNow.Date;
        await _unitOfWork.Allocations.UpdateAsync(allocation);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("EmployeeProjectAllocation", allocationId, "Update", userId);
        return true;
    }
}
