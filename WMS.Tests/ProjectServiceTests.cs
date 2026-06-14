using AutoMapper;
using Moq;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Application.Mappings;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Tests;

public class ProjectServiceTests
{
    [Fact]
    public async Task ApproveAllocation_ReturnsNull_WhenNotPending()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        unitOfWork.Setup(u => u.Allocations.GetByIdAsync(1)).ReturnsAsync(new EmployeeProjectAllocation
        {
            AllocationId = 1, ApprovalStatus = "Approved"
        });
        var queryRepo = new Mock<IProjectQueryRepository>();
        var mapper = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();
        var audit = new AuditService(unitOfWork.Object);
        var service = new ProjectService(unitOfWork.Object, queryRepo.Object, mapper, audit);

        var result = await service.ApproveRejectAllocationAsync(new AllocationApprovalDto
        {
            AllocationId = 1, ApprovalStatus = "Approved", UpdatedBy = "manager"
        }, 1);

        Assert.Null(result);
    }
}
