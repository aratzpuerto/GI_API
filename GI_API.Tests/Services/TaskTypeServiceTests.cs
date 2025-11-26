using GI_API.Database;
using GI_API.Models;
using GI_API.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GI_API.Tests.Services
{
    [Trait("Category", "Services")]
    public class TaskTypeServiceTests
    {
        private TaskTypeService GetService(out GIDbContext context)
        {
            var options = new DbContextOptionsBuilder<GIDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            context = new GIDbContext(options);
            return new TaskTypeService(context);
        }


        // GetAll
        [Fact]
        public async System.Threading.Tasks.Task GetAll_ShouldReturnAllEntities()
        {
            var service = GetService(out var context);
            await service.SetTaskType("A");
            await service.SetTaskType("B");

            var result = service.GetAll();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.Name == "A");
            Assert.Contains(result, t => t.Name == "B");
        }


        // GetById
        [Fact]
        public async System.Threading.Tasks.Task GetById_ShouldReturnEntity_WhenExists()
        {
            var service = GetService(out var context);
            var id = await service.SetTaskType("X");

            var entity = service.GetById(id);

            Assert.NotNull(entity);
            Assert.Equal("X", entity.Name);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenDoesNotExist()
        {
            var service = GetService(out var context);

            var entity = service.GetById(999);

            Assert.Null(entity);
        }


        // SetTaskType
        [Fact]
        public async System.Threading.Tasks.Task SetTaskType_ShouldAddEntity_WhenNameIsValid()
        {
            var service = GetService(out var context);

            var id = await service.SetTaskType("Test");

            Assert.True(id > 0);
            Assert.Single(context.TaskTypes);
            Assert.Equal("Test", context.TaskTypes.First().Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task SetTaskType_ShouldAllowEmptyName()
        {
            var service = GetService(out var context);

            var id = await service.SetTaskType("");

            var entity = context.TaskTypes.Find(id);
            Assert.NotNull(entity);
            Assert.Equal("", entity.Name);
        }


        [Fact]
        public async System.Threading.Tasks.Task SetTaskType_ShouldThrow_WhenNameIsNull()
        {
            var service = GetService(out var context);

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.SetTaskType(null));
        }

        // UpdateTaskType
        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskType_ShouldUpdateName_WhenEntityExists()
        {
            var service = GetService(out var context);
            var id = await service.SetTaskType("Old");

            var (rows, oldValue) = await service.UpdateTaskType(id, "New");


            Assert.Equal(1, rows);
            Assert.Equal("Old", oldValue);
            Assert.Equal("New", context.TaskTypes.Find(id).Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskType_ShouldAllowEmptyName()
        {
            var service = GetService(out var context);
            var id = await service.SetTaskType("Initial");

            var (rows, oldValue) = await service.UpdateTaskType(id, "");

            Assert.Equal(1, rows);
            Assert.Equal("Initial", oldValue);
            Assert.Equal("", context.TaskTypes.Find(id).Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskType_ShouldWork_WhenNameIsSameAsCurrent()
        {
            var service = GetService(out var context);
            var id = await service.SetTaskType("Same");

            var (rows, oldValue) = await service.UpdateTaskType(id, "Same");

            Assert.Equal(1, rows);
            Assert.Equal("Same", oldValue);
            Assert.Equal("Same", context.TaskTypes.Find(id).Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskType_ShouldAllowWhitespaceName()
        {
            var service = GetService(out var context);
            var id = await service.SetTaskType("Initial");

            var (rows, oldValue) = await service.UpdateTaskType(id, "   ");

            Assert.Equal(1, rows);
            Assert.Equal("Initial", oldValue);
            Assert.Equal("   ", context.TaskTypes.Find(id).Name);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskType_ShouldReturnZero_WhenEntityDoesNotExist()
        {
            var service = GetService(out var context);

            var (rows, oldValue) = await service.UpdateTaskType(999, "New");

            Assert.Equal(0, rows);
            Assert.Null(oldValue);
        }

        [Fact]
        public async System.Threading.Tasks.Task UpdateTaskType_ShouldThrow_WhenNameIsNull()
        {
            var service = GetService(out var context);
            var id = await service.SetTaskType("Initial");

            await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateTaskType(id, null));
        }

        // DeleteTaskType
        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskType_ShouldRemoveEntity_WhenEntityExists()
        {
            var service = GetService(out var context);
            var id = await service.SetTaskType("ToDelete");

            var (rows, deletedValue) = await service.DeleteTaskType(id);

            Assert.Equal(1, rows);
            Assert.Equal("ToDelete", deletedValue);
            Assert.Empty(context.TaskTypes);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskType_ShouldReturnZero_WhenEntityDoesNotExist()
        {
            var service = GetService(out var context);

            var (rows, deletedValue) = await service.DeleteTaskType(999);

            Assert.Equal(0, rows);
            Assert.Null(deletedValue);
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskType_ShouldReturnZero_WhenAlreadyDeleted()
        {
            var service = GetService(out var context);
            var id = await service.SetTaskType("ToDelete");

            await service.DeleteTaskType(id);
            var (rows, deletedValue) = await service.DeleteTaskType(id);

            Assert.Equal(0, rows);
            Assert.Null(deletedValue);
        }


    }
}