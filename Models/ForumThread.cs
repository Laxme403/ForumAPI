using System.Collections.Generic;
using System;

namespace dev_forum_api.Models
{
    public class ForumThread
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string Author { get; set; }
        public int CategoryId { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public string Tags { get; set; }
        public int Replies { get; set; }
        public int deleteindex { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
    }
}
