using GI_API.Models;

public interface ITaskTypeService
{
    List<TaskType> GetAll();
    TaskType? GetById(int id);
    Task<int> SetTaskType(string name);
    Task<(int RowsAffected, string OldValue)> UpdateTaskType(int id, string name);
    Task<(int RowsAffected, string DeletedValue)> DeleteTaskType(int id);
}