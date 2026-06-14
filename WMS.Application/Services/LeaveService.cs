using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services;

public class LeaveService : ILeaveService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILeaveQueryRepository _queryRepo;
    private readonly IMapper _mapper;
    private readonly AuditService _audit;

    public LeaveService(IUnitOfWork unitOfWork, ILeaveQueryRepository queryRepo, IMapper mapper, AuditService audit)
    {
        _unitOfWork = unitOfWork;
        _queryRepo = queryRepo;
        _mapper = mapper;
        _audit = audit;
    }

    public async Task<IEnumerable<LeaveDto>> GetAllAsync()
    {
        var leaves = await _queryRepo.GetAllWithDetailsAsync();
        return _mapper.Map<IEnumerable<LeaveDto>>(leaves);
    }

    public async Task<IEnumerable<LeaveDto>> GetByEmployeeAsync(int empId)
    {
        var leaves = await _queryRepo.GetByEmployeeAsync(empId);
        return _mapper.Map<IEnumerable<LeaveDto>>(leaves);
    }

    public async Task<LeaveDto> ApplyAsync(ApplyLeaveDto dto, int userId)
    {
        if (dto.ToDate < dto.FromDate)
            throw new InvalidOperationException("To date must be on or after from date.");

        var leave = _mapper.Map<Leave>(dto);
        leave.Status = "Pending";
        leave.AppliedOn = DateTime.UtcNow;
        await _unitOfWork.Leaves.AddAsync(leave);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Leave", leave.LeaveId, "Insert", userId);

        var leaves = await _queryRepo.GetByEmployeeAsync(dto.EmpId);
        var created = leaves.First(l => l.LeaveId == leave.LeaveId);
        return _mapper.Map<LeaveDto>(created);
    }

    public async Task<bool> CancelAsync(int leaveId, int userId)
    {
        var leave = await _unitOfWork.Leaves.GetByIdAsync(leaveId);
        if (leave == null || leave.Status != "Pending") return false;
        await _unitOfWork.Leaves.DeleteAsync(leave);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Leave", leaveId, "Delete", userId);
        return true;
    }

    public async Task<LeaveDto?> ApproveRejectAsync(LeaveApprovalDto dto, int userId)
    {
        var leave = await _unitOfWork.Leaves.GetByIdAsync(dto.LeaveId);
        if (leave == null || leave.Status != "Pending") return null;

        if (leave.EmpId == dto.ApprovedBy)
            throw new InvalidOperationException("You cannot approve or reject your own leave request.");

        leave.Status = dto.Status;
        leave.ApprovedBy = dto.ApprovedBy;
        leave.ApprovedOn = DateTime.UtcNow;
        await _unitOfWork.Leaves.UpdateAsync(leave);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Leave", leave.LeaveId, "Update", userId);

        var leaves = await _queryRepo.GetAllWithDetailsAsync();
        var updated = leaves.FirstOrDefault(l => l.LeaveId == leave.LeaveId);
        return updated == null ? null : _mapper.Map<LeaveDto>(updated);
    }
}
