using GI_API.Models;
using Microsoft.EntityFrameworkCore;

namespace GI_API.Data
{
    public class TaskTypeContext : DbContext
    {
        public TaskTypeContext(DbContextOptions options) : base(options) { }

        public DbSet<TaskType> TaskTypes { get; set; }


    }
}
