using dev_forum_api.Models;
using dev_forum_api.Interfaces;
using dev_forum_api.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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
    }
}