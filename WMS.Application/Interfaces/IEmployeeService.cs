using WMS.Application.DTOs;

namespace WMS.Application.Interfaces;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetAllAsync();
    Task<EmployeeDto?> GetByIdAsync(int id);
    Task<IEnumerable<EmployeeDto>> SearchAsync(EmployeeSearchDto search);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, int userId);
    Task<EmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto dto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
}
