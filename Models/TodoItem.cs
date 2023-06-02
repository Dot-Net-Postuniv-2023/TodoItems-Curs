namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public List<TodoSubItem>? TodoSubItems { get; set; }
    }
}