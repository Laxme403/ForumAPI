using dev_forum_api.Models;
using dev_forum_api.Interfaces;
using dev_forum_api.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace dev_forum_api.Repositories
{
    public class ThreadRepository : IThreadRepository
    {
        private readonly AppDbContext _context;

        public ThreadRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ForumThread>> GetAllAsync()
        {
            return await _context.Threads.ToListAsync();
        }

        public async Task<ForumThread> GetByIdAsync(int id)
        {
            return await _context.Threads.FindAsync(id);
        }

        public async Task<ForumThread> AddAsync(ForumThread thread)
        {
            _context.Threads.Add(thread);
            await _context.SaveChangesAsync();
            return thread;
        }

        public async Task UpdateAsync(ForumThread thread)
        {
            _context.Entry(thread).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var thread = await _context.Threads.FindAsync(id);
            if (thread != null)
            {
                _context.Threads.Remove(thread);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ForumThread>> GetThreadsLikedByUser(int userId)
        {
            var likedThreadIds = await _context.ThreadLikes
                .Where(l => l.UserId == userId)
                .Select(l => l.ThreadId)
                .ToListAsync();

            return await _context.Threads
                .Where(t => likedThreadIds.Contains(t.Id))
                .ToListAsync();
        }

        public async Task<List<ForumThread>> GetThreadsDislikedByUser(int userId)
        {
            var dislikedThreadIds = await _context.ThreadDislikes
                .Where(d => d.UserId == userId)
                .Select(d => d.ThreadId)
                .ToListAsync();

            return await _context.Threads
                .Where(t => dislikedThreadIds.Contains(t.Id))
                .ToListAsync();
        }

        public async Task<(int likes, int dislikes)> LikeThread(int threadId, int userId)
        {
            // Remove any existing dislike
            var existingDislike = await _context.ThreadDislikes
                .FirstOrDefaultAsync(d => d.ThreadId == threadId && d.UserId == userId);
            if (existingDislike != null)
                _context.ThreadDislikes.Remove(existingDislike);

            // Add like if not already liked
            var alreadyLiked = await _context.ThreadLikes
                .AnyAsync(l => l.ThreadId == threadId && l.UserId == userId);
            if (!alreadyLiked)
                _context.ThreadLikes.Add(new ThreadLike { ThreadId = threadId, UserId = userId });

            // Update thread counts
            var thread = await _context.Threads.FindAsync(threadId);
            if (thread != null)
            {
                thread.Likes = await _context.ThreadLikes.CountAsync(l => l.ThreadId == threadId);
                thread.Dislikes = await _context.ThreadDislikes.CountAsync(d => d.ThreadId == threadId);
            }

            await _context.SaveChangesAsync();
            return (thread?.Likes ?? 0, thread?.Dislikes ?? 0);
        }

        public async Task<(int likes, int dislikes)> DislikeThread(int threadId, int userId)
        {
            // Remove any existing like
            var existingLike = await _context.ThreadLikes
                .FirstOrDefaultAsync(l => l.ThreadId == threadId && l.UserId == userId);
            if (existingLike != null)
                _context.ThreadLikes.Remove(existingLike);

            // Add dislike if not already disliked
            var alreadyDisliked = await _context.ThreadDislikes
                .AnyAsync(d => d.ThreadId == threadId && d.UserId == userId);
            if (!alreadyDisliked)
                _context.ThreadDislikes.Add(new ThreadDislike { ThreadId = threadId, UserId = userId });

            // Update thread counts
            var thread = await _context.Threads.FindAsync(threadId);
            if (thread != null)
            {
                thread.Likes = await _context.ThreadLikes.CountAsync(l => l.ThreadId == threadId);
                thread.Dislikes = await _context.ThreadDislikes.CountAsync(d => d.ThreadId == threadId);
            }

            await _context.SaveChangesAsync();
            return (thread?.Likes ?? 0, thread?.Dislikes ?? 0);
        }
    }
}