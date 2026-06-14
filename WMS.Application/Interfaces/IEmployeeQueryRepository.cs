using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface IEmployeeQueryRepository
{
    Task<List<Employee>> GetAllWithDetailsAsync();
    Task<Employee?> GetByIdWithDetailsAsync(int id);
    Task<List<Employee>> SearchAsync(string? name, int? employeeId, int? departmentId, int? roleId, string? departmentName = null);
}
