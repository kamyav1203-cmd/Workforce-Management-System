using System.ComponentModel.DataAnnotations;

namespace WMS.Domain.Entities;

public class AuditLog
{
    public int AuditId { get; set; }

    [Required]
    public string EntityName { get; set; } = string.Empty;

    public int RecordId { get; set; }

    [Required, MaxLength(20)]
    public string Action { get; set; } = string.Empty;

    public int CreatedBY { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}
