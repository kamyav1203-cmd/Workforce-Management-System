using Microsoft.EntityFrameworkCore;
using WMS.Application.Interfaces;
using WMS.Domain.Entities;
using WMS.Infrastructure.Data;

namespace WMS.Infrastructure.Repositories;

public class ProjectRepository : IProjectQueryRepository
{
    private readonly WmsDbContext _context;

    public ProjectRepository(WmsDbContext context) => _context = context;

    public async Task<List<Project>> GetAllWithClientAsync() =>
        await _context.Projects.Include(p => p.Client).ToListAsync();

    public async Task<Project?> GetByIdWithClientAsync(int id) =>
        await _context.Projects.Include(p => p.Client).FirstOrDefaultAsync(p => p.ProjectId == id);

    public async Task<List<EmployeeProjectAllocation>> GetAllocationsAsync() =>
        await _context.EmployeeProjectAllocations
            .Include(a => a.Employee).Include(a => a.Project)
            .OrderByDescending(a => a.CreateDate).ToListAsync();
}
