namespace dev_forum_api.DTOs
{
    public class ReplyCreateDto
    {
        public string Content { get; set; }
        public int ThreadId { get; set; }
        public int UserId { get; set; }
    }
}