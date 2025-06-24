namespace dev_forum_api.DTOs
{
    public class ReplyDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int ThreadId { get; set; }
        public int UserId { get; set; }
        public string AuthorName { get; set; } // <-- For username
    }
}