using WMS.Application.DTOs;

namespace WMS.Application.Interfaces;

public interface ILeaveService
{
    Task<IEnumerable<LeaveDto>> GetAllAsync();
    Task<IEnumerable<LeaveDto>> GetByEmployeeAsync(int empId);
    Task<LeaveDto> ApplyAsync(ApplyLeaveDto dto, int userId);
    Task<bool> CancelAsync(int leaveId, int userId);
    Task<LeaveDto?> ApproveRejectAsync(LeaveApprovalDto dto, int userId);
}
