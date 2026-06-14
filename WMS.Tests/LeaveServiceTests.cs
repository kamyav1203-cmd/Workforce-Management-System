using AutoMapper;
using Moq;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Application.Mappings;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Tests;

public class LeaveServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<ILeaveQueryRepository> _queryRepo = new();
    private readonly IMapper _mapper;

    public LeaveServiceTests()
    {
        _mapper = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();
    }

    [Fact]
    public async Task ApplyLeave_Throws_WhenToDateBeforeFromDate()
    {
        var audit = new AuditService(_unitOfWork.Object);
        var service = new LeaveService(_unitOfWork.Object, _queryRepo.Object, _mapper, audit);
        var dto = new ApplyLeaveDto
        {
            EmpId = 1, LeaveType = "Casual",
            FromDate = DateTime.Today, ToDate = DateTime.Today.AddDays(-1)
        };
        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ApplyAsync(dto, 1));
    }

    [Fact]
    public async Task ApproveReject_Throws_WhenSelfApprovalAttempted()
    {
        var audit = new AuditService(_unitOfWork.Object);
        var service = new LeaveService(_unitOfWork.Object, _queryRepo.Object, _mapper, audit);
        var leave = new Leave
        {
            LeaveId = 1,
            EmpId = 2, // Manager's Employee ID
            Status = "Pending"
        };
        _unitOfWork.Setup(u => u.Leaves.GetByIdAsync(1)).ReturnsAsync(leave);

        var dto = new LeaveApprovalDto
        {
            LeaveId = 1,
            Status = "Approved",
            ApprovedBy = 2 // Same Employee ID
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.ApproveRejectAsync(dto, 2));
    }
}
