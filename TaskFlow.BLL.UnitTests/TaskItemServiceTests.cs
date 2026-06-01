using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Implementations;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.BLL.UnitTests;

public class TaskItemServiceTests
{
    [Fact]
    public async Task CreateAsync_Should_Add_TaskItem_And_ReturnDto_WithId()
    {
        // Arrange
        var unitOfWork = new Mock<IUnitOfWork>();
        var itemRepo = new Mock<ITaskItemRepository>();
        var tagRepo = new Mock<ITaskTagRepository>();

        unitOfWork.SetupGet(x => x.TaskItemsRepository).Returns(itemRepo.Object);
        unitOfWork.SetupGet(x => x.TaskTagsRepository).Returns(tagRepo.Object);

        itemRepo
            .Setup(r => r.AddAsync(It.IsAny<TaskItemEntity>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        unitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var service = new TaskItemService(unitOfWork.Object);

        var dto = new TaskItemUpsertDto
        {
            ProjectId = 1,
            Name = "New Task",
            Description = "desc",
            Status = TaskFlow.DAL.Entities.TaskStatus.Todo,
            Category = TaskFlow.DAL.Entities.TaskCategory.General,
        };

        // Act
        var result = await service.CreateAsync(dto, CancellationToken.None);

        // Assert - these are expected to fail until CreateAsync is implemented correctly
        itemRepo.Verify(r => r.AddAsync(It.IsAny<TaskItemEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);

        Assert.Equal(dto.Name, result.Name);
        Assert.Equal(dto.ProjectId, result.ProjectId);
    }
}
