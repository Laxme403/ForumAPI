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

        public string Author { get; set; } // or a User object if you want to expand

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public string Tags { get; set; } // comma-separated, or use a related table

        public int Replies { get; set; } // for count only
    }
}
