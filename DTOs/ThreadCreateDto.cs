namespace dev_forum_api.Models
{
    public class ThreadCreateDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string Author { get; set; }
        public string Tags { get; set; }
        // Likes, Dislikes, Replies are usually set to 0 on creation
    }
}