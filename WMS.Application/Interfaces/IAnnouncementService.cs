using WMS.Application.DTOs;

namespace WMS.Application.Interfaces;

public interface IAnnouncementService
{
    Task<IEnumerable<AnnouncementDto>> GetAllAsync();
    Task<IEnumerable<AnnouncementDto>> GetActiveAsync();
    Task<AnnouncementDto> CreateAsync(CreateAnnouncementDto dto, int userId);
    Task<AnnouncementDto?> UpdateAsync(int id, UpdateAnnouncementDto dto, int userId);
    Task<bool> DeleteAsync(int id, int userId);
}
