using Moq;
using TaskFlow.BLL.DTOs;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.BLL.UnitTests;

public class TaskCommentServiceTests
{
    [Fact]
    public async Task CreateAsync_Should_Add_Comment_And_ReturnDto_WithId()
    {
        // Arrange
        var uow = new Mock<IUnitOfWork>();
        var commentsRepo = new Mock<ITaskCommentRepository>();

        uow.SetupGet(x => x.TaskCommentsRepository).Returns(commentsRepo.Object);

        commentsRepo
            .Setup(r => r.AddAsync(It.IsAny<TaskCommentEntity>(), It.IsAny<CancellationToken>()))
            .Callback<TaskCommentEntity, CancellationToken>((e, ct) => { e.Id = 42; })
            .Returns(Task.CompletedTask);

        uow.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var service = new TaskFlow.BLL.Services.Implementations.TaskCommentService(uow.Object);

        var dto = new TaskCommentCreateDto
        {
            TaskItemId = 1,
            Text = "hello"
        };

        // Act
        var result = await service.CreateAsync(dto, "user-1", CancellationToken.None);

        // Assert - we expect created id to be set after save
        commentsRepo.Verify(r => r.AddAsync(It.IsAny<TaskCommentEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result.Id > 0, "Created comment DTO should have a positive Id set after save.");
        Assert.Equal(dto.Text, result.Text);
        Assert.Equal(dto.TaskItemId, result.TaskItemId);
    }
}
