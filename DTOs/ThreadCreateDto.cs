namespace dev_forum_api.Models
{
    public class ThreadCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string Author { get; set; }
        public string Tags { get; set; }
        
        public int deleteindex { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
    }
}