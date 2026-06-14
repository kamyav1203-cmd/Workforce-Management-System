using AutoMapper;
using Moq;
using WMS.Application.Interfaces;
using WMS.Application.Mappings;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Tests;

public class AttendanceServiceTests
{
    [Fact]
    public async Task CheckIn_Throws_WhenAlreadyCheckedIn()
    {
        var unitOfWork = new Mock<IUnitOfWork>();
        var queryRepo = new Mock<IAttendanceQueryRepository>();
        queryRepo.Setup(r => r.GetTodayOpenAsync(1)).ReturnsAsync(new Attendance { AttendanceId = 1 });
        var mapper = new MapperConfiguration(c => c.AddProfile<MappingProfile>()).CreateMapper();
        var audit = new AuditService(unitOfWork.Object);
        var service = new AttendanceService(unitOfWork.Object, queryRepo.Object, mapper, audit);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.CheckInAsync(new WMS.Application.DTOs.CheckInDto { EmpId = 1, WorkMode = "WFO" }, 1));
    }
}
