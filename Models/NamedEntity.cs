using System.ComponentModel.DataAnnotations;

namespace GI_API.Models
{
    public abstract class NamedEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Name { get; set; }
    }

    public class AscensionMaterialType : NamedEntity { }
    public class Day : NamedEntity { }
    public class DomainType : NamedEntity { }
    public class Element : NamedEntity { }
    public class Region : NamedEntity { }
    public class Stat : NamedEntity { }
    public class TaskType : NamedEntity { }
    public class WeaponType : NamedEntity { }




}
