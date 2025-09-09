using GI_API.Models;
using GI_API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;
using System.Runtime.InteropServices;

namespace GI_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        public readonly IConfiguration _configuration;

        public TaskController(IConfiguration configuration) { _configuration = configuration; }

        [HttpGet("GetTasks")]
        public ActionResult<List<Models.Task>> GetTaskTypes()
        {
            List<Models.Task> taskList = new List<Models.Task>();
            taskList = TaskService.GetAll(_configuration);

            return taskList;
        }


        [HttpGet("GetTaskById")]
        public ActionResult<Models.Task> GetTaskTypeById(int id)
        {
            if (id == 0) return BadRequest(new { success = false, message = "id cannot be 0" });

            Models.Task task = TaskService.GetById(id, _configuration);
            if (task == null)
                return NotFound();
            return task;
        }

        [HttpPost("SetTask")]
        public async Task<ActionResult> SetTask(string name, string? description, int typeId, int? recurringEvery, int? showOrder, bool? show, bool? completed, DateTime? completionDate, bool? active)
        {
            if (string.IsNullOrEmpty(name)) return BadRequest(new { success = false, message = "name cannot be empty" });

            int newId = await TaskService.SetTask(name, description, typeId, recurringEvery, showOrder, show, completed, completionDate, active, _configuration);

            return Ok(new
            {
                success = true,
                message = "Task created successfully",
                id = newId,
                name = name,
                decription = description,
                typeId = typeId,
                recurringEvery = recurringEvery,
                showOrder = showOrder,
                show = show,
                completed = completed,
                completionDate = completionDate,
                active = active

            });

        }

        [HttpPut("UpdateTask")]
        public async Task<ActionResult> UpdateTask(int id, string? name, string? description, int? typeId, int? recurringEvery, int? showOrder, bool? show, bool? completed, DateTime? completionDate, bool? active)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "Invalid id" });

            if (name == null &&
                description == null &&
                typeId == null &&
                recurringEvery == null &&
                showOrder == null &&
                show == null &&
                completed == null &&
                completionDate == null &&
                active == null
                ) return BadRequest(new { success = false, message = "At least one field must be provided to update" });

            var rows = await TaskService.UpdateTask(id, name, description, typeId, recurringEvery, showOrder, show, completed, completionDate, active, _configuration);

            if (rows == 0) return NotFound(new { success = false, message = string.Format("Task type with id {0} not found.", id) });

            // Build dynamic response
            dynamic response = new ExpandoObject();
            response.success = true;
            response.message = "Task updated successfully";
            response.id = id;

            if (name != null) response.name = name;
            if (description != null) response.description = description;
            if (typeId != null) response.typeId = typeId;
            if (recurringEvery != null) response.recurringEvery = recurringEvery;
            if (showOrder != null) response.showOrder = showOrder;
            if (show != null) response.show = show;
            if (completed != null) response.completed = completed;
            if (completionDate != null) response.completionDate = completionDate;
            if (active != null) response.active = active;

            response.rowsAffected = rows;

            return Ok(response);
        }

        [HttpDelete("DeleteTask")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            if (id <= 0) return BadRequest(new { success = false, message = "Invalid id" });

            var rows = await TaskService.DeleteTask(id, _configuration);

            if (rows == 0) return NotFound(new { success = false, message = $"Task with id {id} not found" });

            return Ok(new
            {
                success = true,
                message = $"Task with id {id} deleted successfully",
                id = id,
                rowsAffected = rows
            });
        }

    }
}
