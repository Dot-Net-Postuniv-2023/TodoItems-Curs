namespace TodoApi.DTOs 
{
    public class TodoItemDTO 
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public List<TodoSubItemDTO>? TodoSubItems { get; set; }
    }
}