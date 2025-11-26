using GI_API.Database;
using GI_API.Models;
using GI_API.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace GI_API.Tests.Services
{
    public class TaskTypeServiceExceptionTests
    {
        [Fact]
        public async System.Threading.Tasks.Task SetTaskType_ShouldThrow_WhenSaveChangesFails()
        {
            // Arrange: mock DbSet
            var mockSet = new Mock<DbSet<TaskType>>();

            // Arrange: mock DbContext
            var mockContext = new Mock<GIDbContext>(new DbContextOptions<GIDbContext>());
            mockContext.Setup(c => c.TaskTypes).Returns(mockSet.Object);
            mockContext
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException("Save failed"));

            var service = new TaskTypeService(mockContext.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<DbUpdateException>(() => service.SetTaskType("Test"));
            Assert.Equal("Save failed", ex.Message);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskType_ShouldThrow_WhenSaveChangesFails()
        {
            // Arrange
            var taskType = new TaskType { Id = 1, Name = "Old" };

            var mockSet = new Mock<DbSet<TaskType>>();
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                   .ReturnsAsync(taskType);

            var mockContext = new Mock<GIDbContext>(new DbContextOptions<GIDbContext>());
            mockContext.Setup(c => c.TaskTypes).Returns(mockSet.Object);
            mockContext
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException("Save failed"));

            var service = new TaskTypeService(mockContext.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<DbUpdateException>(() => service.UpdateTaskType(1, "New"));
            Assert.Equal("Save failed", ex.Message);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskType_ShouldThrow_WhenSaveChangesFails()
        {
            // Arrange
            var taskType = new TaskType { Id = 1, Name = "ToDelete" };

            var mockSet = new Mock<DbSet<TaskType>>();
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                   .ReturnsAsync(taskType);

            var mockContext = new Mock<GIDbContext>(new DbContextOptions<GIDbContext>());
            mockContext.Setup(c => c.TaskTypes).Returns(mockSet.Object);
            mockContext
                .Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException("Save failed"));

            var service = new TaskTypeService(mockContext.Object);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<DbUpdateException>(() => service.DeleteTaskType(1));
            Assert.Equal("Save failed", ex.Message);
        }
    }
}
