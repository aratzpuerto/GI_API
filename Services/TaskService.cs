using GI_API.Database;
using GI_API.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Runtime.InteropServices;

namespace GI_API.Services
{
    public class TaskService
    {
        private readonly GIDbContext _context;
        public TaskService(GIDbContext context) { _context = context; }
        public List<Models.Task> GetAll()
        {
            return _context.Tasks.ToList();
        }

        public Models.Task? GetById(int id)
        {
            return _context.Tasks.Find(id);
        }

        public async Task<int> SetTask(string name, string? description, int typeId, int? recurringEvery, int? showOrder, bool? show, bool? completed, DateTime? completionDate, bool? active)
        {
            var task = new Models.Task
            {
                Name = name,
                TypeId = typeId,
            };

            if (description != null) task.Description = description;
            if (recurringEvery != null) task.RecurringEvery = recurringEvery;
            if (showOrder != null) task.ShowOrder = showOrder.Value;
            if (show != null) task.Show = show.Value;
            if (completed != null) task.Completed = completed.Value;
            if (completionDate != null) task.CompletionDate = completionDate;
            if (active != null) task.Active = active.Value;
            
            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();
            return task.Id;
        }
        public async Task<int> UpdateTask(int id, string? name, string? description, int? typeId, int? recurringEvery, int? showOrder, bool? show, bool? completed, DateTime? completionDate, bool? active)
        {
            var entity = await _context.Tasks.FindAsync(id);
            if (entity == null)
                return 0;
            if (!string.IsNullOrEmpty(name)) entity.Name = name;
            if (typeId != null) entity.TypeId = (int)typeId;
            if (description != null) entity.Description = description;
            if (recurringEvery != null) entity.RecurringEvery = recurringEvery;
            if (showOrder != null) entity.ShowOrder = showOrder.Value;
            if (show != null) entity.Show = show.Value;
            if (completed != null) entity.Completed = completed.Value;
            if (completionDate != null) entity.CompletionDate = completionDate;
            if (active != null) entity.Active = active.Value;
            await _context.SaveChangesAsync();
            return 1;
        }
        public async Task<int> DeleteTask(int id)
        {
            var entity = await _context.Tasks.FindAsync(id);
            if (entity == null)
                return 0;
            _context.Tasks.Remove(entity);
            await _context.SaveChangesAsync();
            return 1;
        }

    }
}
