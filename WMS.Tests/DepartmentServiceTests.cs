using AutoMapper;
using Moq;
using WMS.Application.DTOs;
using WMS.Application.Mappings;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Tests;

public class DepartmentServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly IMapper _mapper;

    public DepartmentServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task CreateDepartment_ReturnsDto()
    {
        var dept = new Department { DepartmentId = 1, DepartmentName = "IT", Description = "Tech" };
        _unitOfWork.Setup(u => u.Departments.AddAsync(It.IsAny<Department>())).ReturnsAsync((Department d) => { d.DepartmentId = 1; return d; });
        _unitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);
        _unitOfWork.Setup(u => u.AuditLogs.AddAsync(It.IsAny<AuditLog>())).ReturnsAsync((AuditLog a) => a);

        var audit = new AuditService(_unitOfWork.Object);
        var service = new DepartmentService(_unitOfWork.Object, _mapper, audit);

        var result = await service.CreateAsync(new CreateDepartmentDto { DepartmentName = "IT", Description = "Tech" }, 1);

        Assert.Equal("IT", result.DepartmentName);
    }
}
