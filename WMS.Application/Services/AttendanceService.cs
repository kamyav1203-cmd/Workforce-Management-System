using AutoMapper;
using WMS.Application.Reports;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services;

public class AttendanceService : IAttendanceService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAttendanceQueryRepository _queryRepo;
    private readonly IMapper _mapper;
    private readonly AuditService _audit;

    public AttendanceService(IUnitOfWork unitOfWork, IAttendanceQueryRepository queryRepo, IMapper mapper, AuditService audit)
    {
        _unitOfWork = unitOfWork;
        _queryRepo = queryRepo;
        _mapper = mapper;
        _audit = audit;
    }

    public async Task<AttendanceDto> CheckInAsync(CheckInDto dto, int userId)
    {
        var existing = await _queryRepo.GetTodayRecordAsync(dto.EmpId);
        if (existing != null)
            throw new InvalidOperationException("Employee already checked in today.");

        var attendance = new Attendance
        {
            EmpId = dto.EmpId,
            CheckIn = DateTime.UtcNow,
            WorkMode = dto.WorkMode,
            AttendanceDate = DateTime.UtcNow.Date
        };
        await _unitOfWork.Attendances.AddAsync(attendance);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Attendance", attendance.AttendanceId, "Insert", userId);

        var records = await _queryRepo.GetAllWithDetailsAsync();
        var created = records.First(a => a.AttendanceId == attendance.AttendanceId);
        return _mapper.Map<AttendanceDto>(created);
    }

    public async Task<AttendanceDto?> CheckOutAsync(CheckOutDto dto, int userId)
    {
        var attendance = await _unitOfWork.Attendances.GetByIdAsync(dto.AttendanceId);
        if (attendance == null || attendance.CheckOut != null) return null;

        attendance.CheckOut = DateTime.UtcNow;
        attendance.TotalHours = (attendance.CheckOut.Value - attendance.CheckIn).TotalHours;
        await _unitOfWork.Attendances.UpdateAsync(attendance);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Attendance", attendance.AttendanceId, "Update", userId);

        var records = await _queryRepo.GetAllWithDetailsAsync();
        var updated = records.FirstOrDefault(a => a.AttendanceId == attendance.AttendanceId);
        return updated == null ? null : _mapper.Map<AttendanceDto>(updated);
    }

    public async Task<IEnumerable<AttendanceDto>> GetMonthlyAsync(MonthlyAttendanceRequestDto request)
    {
        var records = await _queryRepo.GetMonthlyAsync(request.EmpId, request.Month, request.Year);
        return _mapper.Map<IEnumerable<AttendanceDto>>(records);
    }

    public async Task<IEnumerable<AttendanceDto>> GetAllAsync()
    {
        var records = await _queryRepo.GetAllWithDetailsAsync();
        return _mapper.Map<IEnumerable<AttendanceDto>>(records);
    }

    public async Task<byte[]> GenerateTimesheetReportAsync(int empId, int month, int year)
    {
        var records = await _queryRepo.GetMonthlyAsync(empId, month, year);
        var employee = await _unitOfWork.Employees.GetByIdAsync(empId);
        return TimesheetReportGenerator.GeneratePdf(employee, empId, month, year, records);
    }
}
