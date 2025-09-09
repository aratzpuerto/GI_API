namespace GI_API.Models
{
    public class Task
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? description { get; set; }
        public int typeId { get; set; }
        public int? recurringEvery { get; set; }

        public int showOrder { get; set; }

        public bool show {  get; set; }

        public bool completed {  get; set; }

        public DateTime? completionDate {  get; set; }

        public bool active {  get; set; }
    }
}
