using GI_API.Models;

public interface ITaskService
{
    List<GI_API.Models.Task> GetAll();
    GI_API.Models.Task? GetById(int id);
    Task<int> SetTask(string name, string? description, int typeId, int? recurringEvery, int? showOrder, bool? show, bool? completed, DateTime? completionDate, bool? active);
    Task<int> UpdateTask(int id, string? name, string? description, int? typeId, int? recurringEvery, int? showOrder, bool? show, bool? completed, DateTime? completionDate, bool? active);
    Task<int> DeleteTask(int id);

}
