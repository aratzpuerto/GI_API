using Azure;
using GI_API.Contracts;
using GI_API.Controllers;
using GI_API.Models;
using GI_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace GI_API.Tests.Controllers
{
    [Trait("Category", "Controllers")]
    public class TaskTypeControllerTests
    {
        private readonly Mock<ITaskTypeService> _serviceMock;
        private readonly Mock<ILoggerService> _loggerMock;
        private readonly TaskTypeController _controller;

        public TaskTypeControllerTests()
        {
            _serviceMock = new Mock<ITaskTypeService>();
            _loggerMock = new Mock<ILoggerService>();
            _controller = new TaskTypeController(_serviceMock.Object, _loggerMock.Object);
        }

        // GetTaskTypes

        [Fact]
        public void GetTaskTypes_ShouldReturnList_WhenDataExists()
        {
            _serviceMock.Setup(s => s.GetAll())
                .Returns(new List<TaskType> { new TaskType { Id = 1, Name = "Test" } });

            var result = _controller.GetTaskTypes().Value;

            var value = Assert.IsType<List<TaskType>>(result);
            Assert.Single(value);
            Assert.Equal("Test", value[0].Name);
        }

        [Fact]
        public void GetTaskTypes_ShouldReturnEmptyList_WhenNoDataExists()
        {
            _serviceMock.Setup(s => s.GetAll()).Returns(new List<TaskType>());

            var result = _controller.GetTaskTypes().Value;

            var value = Assert.IsType<List<TaskType>>(result);
            Assert.Empty(value);
        }

        // GetTaskTypeById

        [Fact]
        public void GetTaskTypeById_ShouldReturnEntity_WhenIdExists()
        {
            var taskType = new TaskType { Id = 1, Name = "Existing" };
            _serviceMock.Setup(s => s.GetById(1)).Returns(taskType);

            var result = _controller.GetTaskTypeById(1);

            var okResult = Assert.IsType<ActionResult<TaskType>>(result);
            Assert.Equal(taskType, okResult.Value);
        }

        [Fact]
        public void GetTaskTypeById_ShouldReturnNotFound_WhenIdDoesNotExist()
        {
            _serviceMock.Setup(s => s.GetById(999)).Returns((TaskType)null);

            var result = _controller.GetTaskTypeById(999);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var response = notFoundResult.Value;

            var successProp = response.GetType().GetProperty("success");
            var messageProp = response.GetType().GetProperty("message");

            Assert.False((bool)successProp.GetValue(response));
            Assert.Equal("Task type with id 999 not found.", (string)messageProp.GetValue(response));
        }

        [Fact]
        public void GetTaskTypeById_ShouldReturnBadRequest_WhenIdIsZero()
        {
            var result = _controller.GetTaskTypeById(0);

            var badResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var response = badResult.Value;
            var successProp = response.GetType().GetProperty("success");
            var messageProp = response.GetType().GetProperty("message");

            Assert.NotNull(successProp);
            Assert.NotNull(messageProp);

            Assert.False((bool)successProp.GetValue(response));
            Assert.Equal("id cannot be 0", (string)messageProp.GetValue(response));
        }

        // SetTaskType

        [Fact]
        public async void SetTaskType_ShouldReturnOk_WhenNameIsValid()
        {
            _serviceMock.Setup(s => s.SetTaskType("New"))
                .ReturnsAsync(5);

            var result = await _controller.SetTaskType("New");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = okResult.Value;

            var successProp = response.GetType().GetProperty("success");
            var idProp = response.GetType().GetProperty("id");
            var nameProp = response.GetType().GetProperty("name");

            Assert.True((bool)successProp.GetValue(response));
            Assert.Equal(5, (int)idProp.GetValue(response));
            Assert.Equal("New", (string)nameProp.GetValue(response));
        }

        [Fact]
        public async void SetTaskType_ShouldReturnBadRequest_WhenNameIsEmpty()
        {
            var result = await _controller.SetTaskType("");

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var successProp = badResult.Value.GetType().GetProperty("success");
            var messageProp = badResult.Value.GetType().GetProperty("message");

            Assert.False((bool)successProp.GetValue(badResult.Value));
            Assert.Equal("name cannot be empty", (string)messageProp.GetValue(badResult.Value));
        }

        [Fact]
        public async void SetTaskType_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            _serviceMock.Setup(s => s.SetTaskType("New"))
                .ThrowsAsync(new Exception("Unexpected error"));

            var ex = await Assert.ThrowsAsync<Exception>(() => _controller.SetTaskType("New"));
            Assert.Equal("Unexpected error", ex.Message);
        }

        // UpdateTaskType

        [Fact]
        public async void UpdateTaskType_ShouldReturnOk_WhenEntityExists()
        {
            _serviceMock.Setup(s => s.UpdateTaskType(1, "UpdatedName"))
                .ReturnsAsync((1, "OldName"));

            var result = await _controller.UpdateTaskType(1, "UpdatedName");

            var okResult = Assert.IsType<OkObjectResult>(result);

            var successProp = okResult.Value.GetType().GetProperty("success");
            var messageProp = okResult.Value.GetType().GetProperty("message");
            var idProp = okResult.Value.GetType().GetProperty("id");
            var nameProp = okResult.Value.GetType().GetProperty("name");

            Assert.True((bool)successProp.GetValue(okResult.Value));
            Assert.Equal("Updated from OldName to UpdatedName", (string)messageProp.GetValue(okResult.Value));
            Assert.Equal(1, (int)idProp.GetValue(okResult.Value));
            Assert.Equal("UpdatedName", (string)nameProp.GetValue(okResult.Value));
        }

        [Fact]
        public async void UpdateTaskType_ShouldReturnBadRequest_WhenNameIsEmpty()
        {
            var result = await _controller.UpdateTaskType(1, "");

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var successProp = badResult.Value.GetType().GetProperty("success");
            var messageProp = badResult.Value.GetType().GetProperty("message");

            Assert.False((bool)successProp.GetValue(badResult.Value));
            Assert.Equal("name cannot be empty", (string)messageProp.GetValue(badResult.Value));
        }

        [Fact]
        public async void UpdateTaskType_ShouldReturnBadRequest_WhenIdIsInvalid()
        {
            var result = await _controller.UpdateTaskType(0, "Name");
            var badResult = Assert.IsType<BadRequestObjectResult>(result);

            var successProp = badResult.Value.GetType().GetProperty("success");
            var messageProp = badResult.Value.GetType().GetProperty("message");

            Assert.NotNull(successProp);
            Assert.NotNull(messageProp);

            Assert.False((bool)successProp.GetValue(badResult.Value));
            Assert.Equal("Invalid id", (string)messageProp.GetValue(badResult.Value));
        }

        [Fact]
        public async void UpdateTaskType_ShouldReturnNotFound_WhenEntityDoesNotExist()
        {
            _serviceMock.Setup(s => s.UpdateTaskType(999, "New"))
                .ReturnsAsync((0, null));

            var result = await _controller.UpdateTaskType(999, "New");

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            var successProp = notFoundResult.Value.GetType().GetProperty("success");
            var messageProp = notFoundResult.Value.GetType().GetProperty("message");

            Assert.False((bool)successProp.GetValue(notFoundResult.Value));
            Assert.Equal("Task type with id 999 not found.", (string)messageProp.GetValue(notFoundResult.Value));
        }

        // DeleteTaskType

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskType_ShouldReturnOk_WhenEntityExists()
        {
            _serviceMock.Setup(s => s.DeleteTaskType(1))
                .ReturnsAsync((1, "ToDelete"));

            var result = await _controller.DeleteTaskType(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var successProp = okResult.Value.GetType().GetProperty("success");
            var messageProp = okResult.Value.GetType().GetProperty("message");
            var idProp = okResult.Value.GetType().GetProperty("id");

            Assert.True((bool)successProp.GetValue(okResult.Value));
            Assert.Equal("Deleted 'ToDelete'", (string)messageProp.GetValue(okResult.Value));
            Assert.Equal(1, (int)idProp.GetValue(okResult.Value));
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskType_ShouldReturnBadRequest_WhenIdIsInvalid()
        {
            var result = await _controller.DeleteTaskType(0);
            var badResult = Assert.IsType<BadRequestObjectResult>(result);

            var successProp = badResult.Value.GetType().GetProperty("success");
            var messageProp = badResult.Value.GetType().GetProperty("message");


            Assert.False((bool)successProp.GetValue(badResult.Value));
            Assert.Equal("Invalid id", (string)messageProp.GetValue(badResult.Value));
        }

        [Fact]
        public async System.Threading.Tasks.Task DeleteTaskType_ShouldReturnNotFound_WhenEntityDoesNotExist()
        {
            _serviceMock.Setup(s => s.DeleteTaskType(999))
                .ReturnsAsync((0, (string)null));

            var result = await _controller.DeleteTaskType(999);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            var successProp = notFoundResult.Value.GetType().GetProperty("success");
            var messageProp = notFoundResult.Value.GetType().GetProperty("message");

            Assert.False((bool)successProp.GetValue(notFoundResult.Value));
            Assert.Equal("Task type with id 999 not found", (string)messageProp.GetValue(notFoundResult.Value));
        }

    }
}