using Microsoft.EntityFrameworkCore;
using dev_forum_api.Models;

namespace dev_forum_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<ForumThread> Threads { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<ThreadLike> ThreadLikes { get; set; }
        public DbSet<ThreadDislike> ThreadDislikes { get; set; }
    }
}
