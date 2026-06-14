using WMS.Domain.Entities;

namespace WMS.Application.Interfaces;

public interface ILeaveQueryRepository
{
    Task<List<Leave>> GetAllWithDetailsAsync();
    Task<List<Leave>> GetByEmployeeAsync(int empId);
}
