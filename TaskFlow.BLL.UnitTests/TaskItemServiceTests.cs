using Moq;
using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Implementations;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;
using DalTaskCategory = TaskFlow.DAL.Entities.TaskCategory;
using DalTaskStatus = TaskFlow.DAL.Entities.TaskStatus;

namespace TaskFlow.BLL.UnitTests;

public class TaskItemServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldAttachOnlyExistingTags()
    {
        var taskRepo = new Mock<ITaskItemRepository>();
        var tagRepo = new Mock<IGenericRepository<TaskTagEntity>>();

        TaskItemEntity? captured = null;
        taskRepo.Setup(x => x.AddAsync(It.IsAny<TaskItemEntity>(), It.IsAny<CancellationToken>()))
            .Callback<TaskItemEntity, CancellationToken>((entity, _) =>
            {
                captured = entity;
                entity.Id = 1;
            })
            .Returns(Task.CompletedTask);
        taskRepo.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        taskRepo.Setup(x => x.GetByIdWithDetailsAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => captured);

        tagRepo.Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TaskTagEntity { Id = 1, Name = "backend" });
        tagRepo.Setup(x => x.GetByIdAsync(99, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskTagEntity?)null);

        var sut = new TaskItemService(taskRepo.Object, tagRepo.Object);
        var dto = new TaskItemUpsertDto(10, "Implement DAL", "Repo pattern", DalTaskStatus.InProgress, DalTaskCategory.Feature, null, [1, 99]);

        var result = await sut.CreateAsync(dto);

        Assert.Equal("Implement DAL", result.Name);
        Assert.Single(result.Tags);
        Assert.Equal("backend", result.Tags.First().Name);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSoftDelete_WhenEntityExists()
    {
        var taskRepo = new Mock<ITaskItemRepository>();
        var tagRepo = new Mock<IGenericRepository<TaskTagEntity>>();
        var entity = new TaskItemEntity { Id = 7, Name = "To delete" };

        taskRepo.Setup(x => x.GetByIdAsync(entity.Id, It.IsAny<CancellationToken>())).ReturnsAsync(entity);
        taskRepo.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var sut = new TaskItemService(taskRepo.Object, tagRepo.Object);
        var deleted = await sut.DeleteAsync(entity.Id);

        Assert.True(deleted);
        taskRepo.Verify(x => x.SoftDelete(entity), Times.Once);
    }
}
