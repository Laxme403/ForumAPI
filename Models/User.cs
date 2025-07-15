using System.Collections.Generic;

namespace dev_forum_api.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        
        public List<ForumThread> Threads { get; set; } = new List<ForumThread>();

        public List<Reply> Replies { get; set; } = new List<Reply>();
    }
}
