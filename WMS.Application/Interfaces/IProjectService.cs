using WMS.Application.DTOs;

namespace WMS.Application.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectDto>> GetAllAsync();
    Task<ProjectDto?> GetByIdAsync(int id);
    Task<ProjectDto> CreateAsync(CreateProjectDto dto, int userId);
    Task<ProjectDto?> UpdateAsync(int id, UpdateProjectDto dto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
    Task<IEnumerable<ClientDto>> GetClientsAsync();
    Task<ClientDto> CreateClientAsync(CreateClientDto dto, int userId);
    Task<ClientDto?> UpdateClientAsync(int id, UpdateClientDto dto, int userId);
    Task<bool> DeleteClientAsync(int id, int userId);
    Task<IEnumerable<AllocationDto>> GetAllocationsAsync();
    Task<AllocationDto> AssignEmployeeAsync(CreateAllocationDto dto, int userId);
    Task<AllocationDto?> ApproveRejectAllocationAsync(AllocationApprovalDto dto, int userId);
    Task<bool> CancelAllocationAsync(int allocationId, string updatedBy, int userId);
}
