using System.Collections.Generic;

namespace dev_forum_api.Models
{
    public class Reply
    {
        public int Id { get; set; }

        public string Content { get; set; } = string.Empty;

        // Foreign keys
        public int ThreadId { get; set; }
        public ForumThread Thread { get; set; } = null!;

        public int UserId { get; set; }
        public User Author { get; set; } = null!;
    }
}
