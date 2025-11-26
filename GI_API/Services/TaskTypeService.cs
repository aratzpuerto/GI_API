using GI_API.Database;
using GI_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace GI_API.Services
{
    public class TaskTypeService: ITaskTypeService
    {
        private readonly GIDbContext _context;
        public TaskTypeService(GIDbContext context) { _context = context; }

        public List<TaskType> GetAll()
        {
            return _context.TaskTypes.ToList();
        }
          public TaskType? GetById(int id)
        {
            return _context.TaskTypes.Find(id);
        }

        public async Task<int> SetTaskType(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            var taskType = new TaskType { Name = name };
            _context.TaskTypes.Add(taskType);
            await _context.SaveChangesAsync();
            return taskType.Id;
        }

        public async Task<(int RowsAffected, string OldValue)> UpdateTaskType(int id, string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            var entity = await _context.TaskTypes.FindAsync(id);
            if (entity == null)
                return (0, null);

            var oldValue = entity.Name;
            entity.Name = name;
            await _context.SaveChangesAsync();

            return (1, oldValue);
        }

        public async Task<(int RowsAffected, string DeletedValue)> DeleteTaskType(int id)
        {
            var entity = await _context.TaskTypes.FindAsync(id);
            if (entity == null)
                return (0, null);

            var deletedValue = entity.Name;
            _context.TaskTypes.Remove(entity);
            await _context.SaveChangesAsync();

            return (1, deletedValue);
        }
    }
    }