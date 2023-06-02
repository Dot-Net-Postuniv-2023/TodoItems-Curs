using System.Text.Json.Serialization;

namespace TodoApi.Models
{
    public class TodoSubItem
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public DateTime? DateAdded { get; set; }
        public DateTime? DateCompleted { get; set; }

        public int Priority { get; set; }


        [JsonIgnore]
        public TodoItem? TodoItem { get; set; }
        public long? TodoItemId { get; set; }
    }
}