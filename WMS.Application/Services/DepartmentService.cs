using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly AuditService _audit;

    public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper, AuditService audit)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _audit = audit;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _unitOfWork.Departments.GetAllAsync();
        return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        var dept = await _unitOfWork.Departments.GetByIdAsync(id);
        return dept == null ? null : _mapper.Map<DepartmentDto>(dept);
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto, int userId)
    {
        var dept = _mapper.Map<Department>(dto);
        dept.CreatedOn = DateTime.UtcNow;
        await _unitOfWork.Departments.AddAsync(dept);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Department", dept.DepartmentId, "Insert", userId);
        return _mapper.Map<DepartmentDto>(dept);
    }

    public async Task<DepartmentDto?> UpdateAsync(int id, UpdateDepartmentDto dto, int userId)
    {
        var dept = await _unitOfWork.Departments.GetByIdAsync(id);
        if (dept == null) return null;
        _mapper.Map(dto, dept);
        await _unitOfWork.Departments.UpdateAsync(dept);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Department", id, "Update", userId);
        return _mapper.Map<DepartmentDto>(dept);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var dept = await _unitOfWork.Departments.GetByIdAsync(id);
        if (dept == null) return false;
        await _unitOfWork.Departments.DeleteAsync(dept);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Department", id, "Delete", userId);
        return true;
    }
}
