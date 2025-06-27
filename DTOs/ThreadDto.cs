namespace dev_forum_api.Models
{
    public class ThreadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string Author { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public string Tags { get; set; }
        public int Replies { get; set; }
        public int deleteindex { get; set; } // <-- Add this line
    }
}