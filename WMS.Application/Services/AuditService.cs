using WMS.Domain.Entities;
using WMS.Domain.Interfaces;
using WMS.Application.DTOs;

namespace WMS.Application.Services;

public class AuditService
{
    private readonly IUnitOfWork _unitOfWork;

    public AuditService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task LogAsync(string entityName, int recordId, string action, int userId)
    {
        await _unitOfWork.AuditLogs.AddAsync(new AuditLog
        {
            EntityName = entityName,
            RecordId = recordId,
            Action = action,
            CreatedBY = userId,
            CreatedOn = DateTime.UtcNow
        });
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<AuditLogDto>> GetAuditLogsAsync()
    {
        var logs = (await _unitOfWork.AuditLogs.GetAllAsync()).ToList();
        var users = (await _unitOfWork.Users.GetAllAsync()).ToList();

        var userDict = users.ToDictionary(u => u.UserId, u => u.Username);

        return logs.Select(l => new AuditLogDto
        {
            AuditId = l.AuditId,
            EntityName = l.EntityName,
            RecordId = l.RecordId,
            Action = l.Action,
            CreatedBY = l.CreatedBY,
            Username = userDict.TryGetValue(l.CreatedBY, out var uname) ? uname : $"User {l.CreatedBY}",
            CreatedOn = l.CreatedOn
        }).OrderByDescending(l => l.CreatedOn);
    }
}
