using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmployeeQueryRepository _queryRepo;
    private readonly IMapper _mapper;
    private readonly AuditService _audit;

    public EmployeeService(IUnitOfWork unitOfWork, IEmployeeQueryRepository queryRepo, IMapper mapper, AuditService audit)
    {
        _unitOfWork = unitOfWork;
        _queryRepo = queryRepo;
        _mapper = mapper;
        _audit = audit;
    }

    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _queryRepo.GetAllWithDetailsAsync();
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        var employee = await _queryRepo.GetByIdWithDetailsAsync(id);
        return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<IEnumerable<EmployeeDto>> SearchAsync(EmployeeSearchDto search)
    {
        var employees = await _queryRepo.SearchAsync(search.Name, search.EmployeeId, search.DepartmentId, search.RoleId, search.DepartmentName);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, int userId)
    {
        ValidateAge(dto.DOB);
        var employee = _mapper.Map<Employee>(dto);
        employee.CreatedOn = DateTime.UtcNow;
        await _unitOfWork.Employees.AddAsync(employee);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Employee", employee.EmployeeId, "Insert", userId);
        var created = await _queryRepo.GetByIdWithDetailsAsync(employee.EmployeeId);
        return _mapper.Map<EmployeeDto>(created!);
    }

    public async Task<EmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto dto, int userId)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        if (employee == null) return null;
        ValidateAge(dto.DOB);
        _mapper.Map(dto, employee);
        employee.UpdatedOn = DateTime.UtcNow;
        await _unitOfWork.Employees.UpdateAsync(employee);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Employee", id, "Update", userId);
        var updated = await _queryRepo.GetByIdWithDetailsAsync(id);
        return _mapper.Map<EmployeeDto>(updated!);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var employee = await _unitOfWork.Employees.GetByIdAsync(id);
        if (employee == null) return false;
        employee.Status = "Inactive";
        employee.UpdatedOn = DateTime.UtcNow;
        await _unitOfWork.Employees.UpdateAsync(employee);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Employee", id, "Delete", userId);
        return true;
    }

    private static void ValidateAge(DateTime dob)
    {
        var age = DateTime.Today.Year - dob.Year;
        if (dob.Date > DateTime.Today.AddYears(-age)) age--;
        if (age < 18) throw new InvalidOperationException("Employee must be at least 18 years old.");
    }
}
