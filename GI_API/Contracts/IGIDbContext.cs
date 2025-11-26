using GI_API.Models;
using Microsoft.EntityFrameworkCore;

public interface IGIDbContext
{
    DbSet<TaskType> TaskTypes { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}