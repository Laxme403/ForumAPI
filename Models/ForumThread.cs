using System.Collections.Generic;

namespace dev_forum_api.Models
{
    public class ForumThread
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // Foreign key
        public int UserId { get; set; }

        public User Author { get; set; } = null!;

        public List<Reply> Replies { get; set; } = new List<Reply>();
    }
}
