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
            ActionResult result;
            _logger.LogInfo(string.Format("GetTaskTypeById/{0}:", id));

            if (id == 0) { 
                result = BadRequest(new { success = false, message = "id cannot be 0" });
                _logger.LogInfo(JsonConvert.SerializeObject(result).ToString());
                return result;
            }

            TaskType taskType = TaskTypeService.GetById(id, _configuration);

            _logger.LogInfo(JsonConvert.SerializeObject(taskType).ToString());
            return taskType;
        }

        [HttpPost("SetTaskType")]
        public async Task<ActionResult> SetTaskType(string name)
        {
            _logger.LogInfo(string.Format("SetTaskType/{0}:", name));

            if (string.IsNullOrEmpty(name)) return BadRequest(new { success = false, message = "name cannot be empty" });

            int newId = await TaskTypeService.SetTaskType(name, _configuration);

            var result = Ok(new
            {
                success = true,
                message = "Task type created successfully",
                id = newId,
                name = name
            });

            _logger.LogInfo(JsonConvert.SerializeObject(result).ToString());

            return result;

        }

        [HttpPut("UpdateTaskType")]
        public async Task<ActionResult> UpdateTaskType(int id, string name)
        {
            ActionResult result;
            _logger.LogInfo(string.Format("UpdateTaskType/{0}/{1}:", id, name));
            
            if (id <= 0) {
                
                result = BadRequest(new { success = false, message = "Invalid id" });
                _logger.LogInfo(JsonConvert.SerializeObject(result).ToString());

                return BadRequest(result); 
            }
            if (string.IsNullOrEmpty(name)) { 
            
                result = BadRequest(new { success = false, message = "name cannot be empty" });
                _logger.LogInfo(JsonConvert.SerializeObject(result).ToString());

                return result;
            
            }

            var (rows, oldValue) = await TaskTypeService.UpdateTaskType(id, name, _configuration);

            if (rows == 0) { 
                result = NotFound(new { success = false, message = string.Format("Task type with id {0} not found.", id) });
                _logger.LogInfo(JsonConvert.SerializeObject(result).ToString());

                return result;
            }

            result = Ok(new
            {
                success = true,
                message = string.Format("Task type updated successfully from {0} to {1}", oldValue, name),
                id = id,
                name = name
            });
            
            _logger.LogInfo(JsonConvert.SerializeObject(result).ToString());

            return result;

        }

        [HttpDelete("DeleteTaskType")]
        public async Task<ActionResult> DeleteTaskType(int id)
        {
            ActionResult result;
            _logger.LogInfo(string.Format("DeleteTaskType/{0}:", id));

            if (id <= 0) { 
                result = BadRequest(new { success = false, message = "Invalid id" });
                _logger.LogInfo(JsonConvert.SerializeObject(result).ToString());
                return result;
            }

            var (rows, deletedValue) = await TaskTypeService.DeleteTaskType(id, _configuration);

            if (rows == 0) { 
                result = NotFound(new { success = false, message = $"Task type with id {id} not found" });
                _logger.LogInfo(JsonConvert.SerializeObject(result).ToString());
                return result;
            }

            result = Ok(new
            {
                success = true,
                message = $"Task type '{deletedValue}' with id {id} deleted successfully",
                id = id,
                deletedValue
            });

            _logger.LogInfo(JsonConvert.SerializeObject(result).ToString());
            return result;
        }

    }
}
