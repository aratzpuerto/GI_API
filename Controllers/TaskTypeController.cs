using GI_API.Contracts;
using GI_API.Models;
using GI_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;


namespace GI_API.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class TaskTypeController : ControllerBase
    {
        private readonly ILoggerService _logger;
        public readonly IConfiguration _configuration;

        public TaskTypeController(IConfiguration configuration, ILoggerService logger) 
        { 
            _configuration = configuration; 
            _logger = logger;
        }

        [HttpGet("GetTaskTypes")]
        public ActionResult<List<TaskType>> GetTaskTypes()
        {
            _logger.LogInfo("GetTaskTypes:");
            List<TaskType> taskTypes = new List<TaskType>();
            taskTypes = TaskTypeService.GetAll(_configuration);
            //return JsonConvert.SerializeObject(taskTypes).ToString();

            _logger.LogInfo(JsonConvert.SerializeObject(taskTypes).ToString());

            return taskTypes;

        }

        [HttpGet("GetTaskTypeById")]
        public ActionResult<TaskType> GetTaskTypeById(int id)
        {
            if (id == 0) return BadRequest(new { success = false, message = "id cannot be 0" });

            TaskType taskType = TaskTypeService.GetById(id, _configuration);
            if (taskType == null)
                return NotFound();
            return taskType;
        }

        [HttpPost("SetTaskType")]
        public async Task<ActionResult> SetTaskType(string name)
        {
            if (string.IsNullOrEmpty(name)) return BadRequest(new { success = false, message = "name cannot be empty" });

            int newId = await TaskTypeService.SetTaskType(name, _configuration);

            return Ok(new
            {
                success = true,
                message = "Task type created successfully",
                id = newId,
                name = name
            });

        }

        [HttpPut("UpdateTaskType")]
        public async Task<ActionResult> UpdateTaskType(int id, string name)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "Invalid id" });
            if (string.IsNullOrEmpty(name)) return BadRequest(new { success = false, message = "name cannot be empty" });

            var (rows, oldValue) = await TaskTypeService.UpdateTaskType(id, name, _configuration);

            if (rows == 0) return NotFound(new { success = false, message = string.Format("Task type with id {0} not found.", id) });

            return Ok(new
            {
                success = true,
                message = string.Format("Task type updated successfully from {0} to {1}", oldValue, name),
                id = id,
                name = name
            });

        }

        [HttpDelete("DeleteTaskType")]
        public async Task<ActionResult> DeleteTaskType(int id)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "Invalid id" });

            var (rows, deletedValue) = await TaskTypeService.DeleteTaskType(id, _configuration);

            if (rows == 0) return NotFound(new { success = false, message = $"Task type with id {id} not found" });

            return Ok(new
            {
                success = true,
                message = $"Task type '{deletedValue}' with id {id} deleted successfully",
                id = id,
                deletedValue
            });
        }

    }
}
