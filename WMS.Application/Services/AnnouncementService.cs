using AutoMapper;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Application.Services;

public class AnnouncementService : IAnnouncementService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly AuditService _audit;

    public AnnouncementService(IUnitOfWork unitOfWork, IMapper mapper, AuditService audit)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _audit = audit;
    }

    public async Task<IEnumerable<AnnouncementDto>> GetAllAsync()
    {
        var announcements = (await _unitOfWork.Announcements.GetAllAsync()).OrderByDescending(a => a.CreatedOn);
        return _mapper.Map<IEnumerable<AnnouncementDto>>(announcements);
    }

    public async Task<IEnumerable<AnnouncementDto>> GetActiveAsync()
    {
        var announcements = (await _unitOfWork.Announcements.FindAsync(a => a.IsActive))
            .OrderByDescending(a => a.CreatedOn);
        return _mapper.Map<IEnumerable<AnnouncementDto>>(announcements);
    }

    public async Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto, int userId)
    {
        var announcement = _mapper.Map<Announcement>(dto);
        announcement.CreatedOn = DateTime.UtcNow;
        announcement.IsActive = true;
        await _unitOfWork.Announcements.AddAsync(announcement);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Announcement", announcement.AnnouncementId, "Insert", userId);
        return _mapper.Map<AnnouncementDto>(announcement);
    }

    public async Task<AnnouncementDto?> UpdateAsync(int id, UpdateAnnouncementDto dto, int userId)
    {
        var announcement = await _unitOfWork.Announcements.GetByIdAsync(id);
        if (announcement == null) return null;
        announcement.Title = dto.Title;
        announcement.Message = dto.Message;
        announcement.IsActive = dto.IsActive;
        await _unitOfWork.Announcements.UpdateAsync(announcement);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Announcement", id, "Update", userId);
        return _mapper.Map<AnnouncementDto>(announcement);
    }

    public async Task<bool> DeleteAsync(int id, int userId)
    {
        var announcement = await _unitOfWork.Announcements.GetByIdAsync(id);
        if (announcement == null) return false;
        announcement.IsActive = false;
        await _unitOfWork.Announcements.UpdateAsync(announcement);
        await _unitOfWork.SaveChangesAsync();
        await _audit.LogAsync("Announcement", id, "Delete", userId);
        return true;
    }
}
