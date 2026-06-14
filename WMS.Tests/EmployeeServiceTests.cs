using AutoMapper;
using Moq;
using WMS.Application.DTOs;
using WMS.Application.Interfaces;
using WMS.Application.Mappings;
using WMS.Application.Services;
using WMS.Domain.Entities;
using WMS.Domain.Interfaces;

namespace WMS.Tests;

public class EmployeeServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<IEmployeeQueryRepository> _queryRepo = new();
    private readonly IMapper _mapper;

    public EmployeeServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public async Task CreateEmployee_Throws_WhenUnder18()
    {
        var audit = new AuditService(_unitOfWork.Object);
        var service = new EmployeeService(_unitOfWork.Object, _queryRepo.Object, _mapper, audit);

        var dto = new CreateEmployeeDto
        {
            FirstName = "Test", LastName = "User", Email = "test@test.com",
            PhoneNumber = "1234567890", Gender = "M",
            DOB = DateTime.Today.AddYears(-10),
            DOJ = DateTime.Today, DepartmentId = 1, RoleId = 1
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() => service.CreateAsync(dto, 1));
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenNotFound()
    {
        _queryRepo.Setup(r => r.GetByIdWithDetailsAsync(999)).ReturnsAsync((Employee?)null);
        var audit = new AuditService(_unitOfWork.Object);
        var service = new EmployeeService(_unitOfWork.Object, _queryRepo.Object, _mapper, audit);

        var result = await service.GetByIdAsync(999);
        Assert.Null(result);
    }
}
