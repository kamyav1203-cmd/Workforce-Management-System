using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface IProjectQueryRepository
{
    Task<List<Project>> GetAllWithClientAsync();
    Task<Project?> GetByIdWithClientAsync(int id);
    Task<List<EmployeeProjectAllocation>> GetAllocationsAsync();
}
