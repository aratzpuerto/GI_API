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
        private readonly ITaskTypeService _service;
                
        public TaskTypeController(ITaskTypeService service, ILoggerService logger) 
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("GetTaskTypes")]
        public ActionResult<List<TaskType>> GetTaskTypes()
        {
            return _service.GetAll();
        }

        [HttpGet("GetTaskTypeById")]
        public ActionResult<TaskType> GetTaskTypeById(int id)
        {
            if (id == 0)
                return BadRequest(new { success = false, message = "id cannot be 0" });

            var taskType = _service.GetById(id);
            if (taskType == null)
                return NotFound(new { success = false, message = $"Task type with id {id} not found." });

            return taskType;
        }

        [HttpPost("SetTaskType")]
        public async Task<ActionResult> SetTaskType(string name)
        {
            if (string.IsNullOrEmpty(name))
                return BadRequest(new { success = false, message = "name cannot be empty" });

            int newId = await _service.SetTaskType(name);

            return Ok(new { success = true, id = newId, name });
        }

        [HttpPut("UpdateTaskType")]
        public async Task<ActionResult> UpdateTaskType(int id, string name)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "Invalid id" });
            if (string.IsNullOrEmpty(name)) return BadRequest(new { success = false, message = "name cannot be empty" });

            var (rows, oldValue) = await _service.UpdateTaskType(id, name);

            if (rows == 0)
                return NotFound(new { success = false, message = $"Task type with id {id} not found." });

            return Ok(new { success = true, message = $"Updated from {oldValue} to {name}", id, name });
        }

        [HttpDelete("DeleteTaskType")]
        public async Task<ActionResult> DeleteTaskType(int id)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "Invalid id" });

            var (rows, deletedValue) = await _service.DeleteTaskType(id);

            if (rows == 0)
                return NotFound(new { success = false, message = $"Task type with id {id} not found" });

            return Ok(new { success = true, message = $"Deleted '{deletedValue}'", id });
        }
    }
}
