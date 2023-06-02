namespace TodoApi.DTOs 
{
    public class TodoSubItemDTO 
    {
        public long? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public DateTime? DateCompleted { get; set; }

        public int Priority { get; set; }
    }
}