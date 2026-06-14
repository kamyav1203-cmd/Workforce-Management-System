using WMS.Application.DTOs;

namespace WMS.Application.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(int id);
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto, int userId);
    Task<DepartmentDto?> UpdateAsync(int id, UpdateDepartmentDto dto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
}
