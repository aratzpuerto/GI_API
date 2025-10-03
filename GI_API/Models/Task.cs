using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GI_API.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(5000)]
        public string Name { get; set; }

        [Column(TypeName = "varchar(max)")]
        public string? Description { get; set; }

        // Foreign key to TaskType
        [ForeignKey(nameof(TaskType))]
        public int TypeId { get; set; }

        public TaskType TaskType { get; set; } = null!; 

        public int? RecurringEvery { get; set; }

        public int? ShowOrder { get; set; }

        public bool? Show { get; set; }

        public bool? Completed { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CompletionDate { get; set; }

        public bool? Active { get; set; }
    }
}
