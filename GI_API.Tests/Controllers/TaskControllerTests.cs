using Azure;
using GI_API.Contracts;
using GI_API.Controllers;
using GI_API.Models;
using GI_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GI_API.Tests.Controllers
{
    [Trait("Category", "Controllers")]
    public class TaskControllerTests
    {
        private readonly Mock<ITaskService> _serviceMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _serviceMock = new Mock<ITaskService>();
            _loggerMock = new Mock<ILoggerService>();
            _controller = new TaskController(_serviceMock.Object, _loggerMock.Object);
        }

        // GetTasks
        [Fact]
        public void GetTasks_ShouldReturnList_WhenDataExists()
        {
            _serviceMock.Setup(s => s.GetAll())
                .Returns(new List<Models.Task> { new Models.Task { Id = 1, Name = "Test", TypeId = 1 } });

            var result = _controller.GetTasks().Value;

            var value = Assert.IsType<List<Models.Task>>(result);
            Assert.Single(value);
            Assert.Equal("Test", value[0].Name);

        }

        [Fact]
        public void GetTasks_ShouldReturnEmptyList_WhenNoDataExists()
        {
            _serviceMock.Setup(s => s.GetAll())
                .Returns(new List<Models.Task>());

            var result = _controller.GetTasks().Value;
            var value = Assert.IsType<List<Models.Task>>(result);
            Assert.Empty(value);

        }

        // GetTasksById
        [Fact]
        public void GetTasksById_ShouldRaiseBadRequest_WhenIdIsZero()
        {
            var result = _controller.GetTasksById(0);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            var successProp = badRequest.Value.GetType().GetProperty("success");
            var messageProp = badRequest.Value.GetType().GetProperty("message");

            Assert.False((bool)successProp.GetValue(badRequest.Value));
            Assert.Equal("id cannot be 0", (string)messageProp.GetValue(badRequest.Value));
        }

        [Fact]
        public void GetTasksById_ShouldRaiseNotFound_WhenIdDoesNotExist()
        {
            _serviceMock.Setup(s => s.GetById(999)).Returns((Models.Task)null);

            var result = _controller.GetTasksById(999);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var successProp = notFoundResult.Value.GetType().GetProperty("success");
            var messageProp = notFoundResult.Value.GetType().GetProperty("message");

            Assert.False((bool)successProp.GetValue(notFoundResult.Value));
            Assert.Equal("Task with id 999 not found.", (string)messageProp.GetValue(notFoundResult.Value));
        }

        [Fact]
        public void GetTasksById_ShouldReturnTask_WhenIdExists()
        {
            var task = new Models.Task { Id = 1, Name = "Test", TypeId = 1 };
            _serviceMock.Setup(s => s.GetById(1))
                .Returns(task);

            var result = _controller.GetTasksById(1);

            var okResult = Assert.IsType<ActionResult<Models.Task>>(result);
            Assert.Equal(task, okResult.Value);
        }

        // CreateTask
        
        [Fact]
        public async void CreateTask_ShouldRaiseBadRequest_IfNameIsEmpty()
        {
            var result = await _controller.CreateTask(string.Empty, null, 1, null,null,null,null,null,null);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var successProp = badRequest.Value.GetType().GetProperty("success");
            var messageProp = badRequest.Value.GetType().GetProperty("message");

            Assert.False((bool)successProp.GetValue(badRequest.Value));
            Assert.Equal("name cannot be empty", (string)messageProp.GetValue(badRequest.Value));

        }

        [Fact]
        public async void CreateTask_ShouldReturnOk_WhenTaskIsValid()
        {
            _serviceMock.Setup(s => s.SetTask("New", null, 1, null, null, null, null, null, null))
                .ReturnsAsync(0);

            var result = await _controller.CreateTask("New", null, 1, null, null, null, null, null, null);
            var okResult = Assert.IsType<OkObjectResult>(result);

            var successProp = okResult.Value.GetType().GetProperty("success");
            var idProp = okResult.Value.GetType().GetProperty("id");
            var nameProp = okResult.Value.GetType().GetProperty("name");

            Assert.True((bool)successProp.GetValue(okResult.Value));
            Assert.Equal(0, (int)idProp.GetValue(okResult.Value));
            Assert.Equal("New", (string)nameProp.GetValue(okResult.Value));

        }


        //[Fact]
        //public async void SetTaskType_ShouldReturnInternalServerError_WhenExceptionOccurs()
        //{
        //    _serviceMock.Setup(s => s.SetTaskType("New"))
        //        .ThrowsAsync(new Exception("Unexpected error"));

        //    var ex = await Assert.ThrowsAsync<Exception>(() => _controller.SetTaskType("New"));
        //    Assert.Equal("Unexpected error", ex.Message);
        //}

        //// UpdateTaskType

        //[Fact]
        //public async void UpdateTaskType_ShouldReturnOk_WhenEntityExists()
        //{
        //    _serviceMock.Setup(s => s.UpdateTaskType(1, "UpdatedName"))
        //        .ReturnsAsync((1, "OldName"));

        //    var result = await _controller.UpdateTaskType(1, "UpdatedName");

        //    var okResult = Assert.IsType<OkObjectResult>(result);

        //    var successProp = okResult.Value.GetType().GetProperty("success");
        //    var messageProp = okResult.Value.GetType().GetProperty("message");
        //    var idProp = okResult.Value.GetType().GetProperty("id");
        //    var nameProp = okResult.Value.GetType().GetProperty("name");

        //    Assert.True((bool)successProp.GetValue(okResult.Value));
        //    Assert.Equal("Updated from OldName to UpdatedName", (string)messageProp.GetValue(okResult.Value));
        //    Assert.Equal(1, (int)idProp.GetValue(okResult.Value));
        //    Assert.Equal("UpdatedName", (string)nameProp.GetValue(okResult.Value));
        //}

        //[Fact]
        //public async void UpdateTaskType_ShouldReturnBadRequest_WhenNameIsEmpty()
        //{
        //    var result = await _controller.UpdateTaskType(1, "");

        //    var badResult = Assert.IsType<BadRequestObjectResult>(result);
        //    var successProp = badResult.Value.GetType().GetProperty("success");
        //    var messageProp = badResult.Value.GetType().GetProperty("message");

        //    Assert.False((bool)successProp.GetValue(badResult.Value));
        //    Assert.Equal("name cannot be empty", (string)messageProp.GetValue(badResult.Value));
        //}

        //[Fact]
        //public async void UpdateTaskType_ShouldReturnBadRequest_WhenIdIsInvalid()
        //{
        //    var result = await _controller.UpdateTaskType(0, "Name");
        //    var badResult = Assert.IsType<BadRequestObjectResult>(result);

        //    var successProp = badResult.Value.GetType().GetProperty("success");
        //    var messageProp = badResult.Value.GetType().GetProperty("message");

        //    Assert.NotNull(successProp);
        //    Assert.NotNull(messageProp);

        //    Assert.False((bool)successProp.GetValue(badResult.Value));
        //    Assert.Equal("Invalid id", (string)messageProp.GetValue(badResult.Value));
        //}

        //[Fact]
        //public async void UpdateTaskType_ShouldReturnNotFound_WhenEntityDoesNotExist()
        //{
        //    _serviceMock.Setup(s => s.UpdateTaskType(999, "New"))
        //        .ReturnsAsync((0, null));

        //    var result = await _controller.UpdateTaskType(999, "New");

        //    var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        //    var successProp = notFoundResult.Value.GetType().GetProperty("success");
        //    var messageProp = notFoundResult.Value.GetType().GetProperty("message");

        //    Assert.False((bool)successProp.GetValue(notFoundResult.Value));
        //    Assert.Equal("Task type with id 999 not found.", (string)messageProp.GetValue(notFoundResult.Value));
        //}

        //// DeleteTaskType

        //[Fact]
        //public async System.Threading.Tasks.Task DeleteTaskType_ShouldReturnOk_WhenEntityExists()
        //{
        //    _serviceMock.Setup(s => s.DeleteTaskType(1))
        //        .ReturnsAsync((1, "ToDelete"));

        //    var result = await _controller.DeleteTaskType(1);

        //    var okResult = Assert.IsType<OkObjectResult>(result);

        //    var successProp = okResult.Value.GetType().GetProperty("success");
        //    var messageProp = okResult.Value.GetType().GetProperty("message");
        //    var idProp = okResult.Value.GetType().GetProperty("id");

        //    Assert.True((bool)successProp.GetValue(okResult.Value));
        //    Assert.Equal("Deleted 'ToDelete'", (string)messageProp.GetValue(okResult.Value));
        //    Assert.Equal(1, (int)idProp.GetValue(okResult.Value));
        //}

        //[Fact]
        //public async System.Threading.Tasks.Task DeleteTaskType_ShouldReturnBadRequest_WhenIdIsInvalid()
        //{
        //    var result = await _controller.DeleteTaskType(0);
        //    var badResult = Assert.IsType<BadRequestObjectResult>(result);

        //    var successProp = badResult.Value.GetType().GetProperty("success");
        //    var messageProp = badResult.Value.GetType().GetProperty("message");


        //    Assert.False((bool)successProp.GetValue(badResult.Value));
        //    Assert.Equal("Invalid id", (string)messageProp.GetValue(badResult.Value));
        //}

        //[Fact]
        //public async System.Threading.Tasks.Task DeleteTaskType_ShouldReturnNotFound_WhenEntityDoesNotExist()
        //{
        //    _serviceMock.Setup(s => s.DeleteTaskType(999))
        //        .ReturnsAsync((0, (string)null));

        //    var result = await _controller.DeleteTaskType(999);

        //    var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        //    var successProp = notFoundResult.Value.GetType().GetProperty("success");
        //    var messageProp = notFoundResult.Value.GetType().GetProperty("message");

        //    Assert.False((bool)successProp.GetValue(notFoundResult.Value));
        //    Assert.Equal("Task type with id 999 not found", (string)messageProp.GetValue(notFoundResult.Value));
        //}

    }
}