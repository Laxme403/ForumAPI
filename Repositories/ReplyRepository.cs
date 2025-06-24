using dev_forum_api.DTOs;
using dev_forum_api.Interfaces;
using dev_forum_api.Models;
using dev_forum_api.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dev_forum_api.Repositories
{
    public class ReplyRepository : IReplyRepository
    {
        private readonly AppDbContext _context;

        public ReplyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Reply> AddAsync(Reply reply)
        {
            _context.Replies.Add(reply);
            await _context.SaveChangesAsync();
            return reply;
        }

        public async Task<IEnumerable<Reply>> GetByThreadIdAsync(int threadId)
        {
            return await _context.Replies
                .Where(r => r.ThreadId == threadId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reply>> GetAllAsync()
        {
            return await _context.Replies.ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var reply = await _context.Replies.FindAsync(id);
            if (reply != null)
            {
                _context.Replies.Remove(reply);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<ReplyDto>> GetDtosByThreadIdAsync(int threadId)
        {
            return await _context.Replies
                .Where(r => r.ThreadId == threadId)
                .Include(r => r.Author)
                .Select(r => new ReplyDto
                {
                    Id = r.Id,
                    Content = r.Content,
                    ThreadId = r.ThreadId,
                    UserId = r.UserId,
                    AuthorName = r.Author != null ? r.Author.Username : "Unknown"
                })
                .ToListAsync();
        }
    }
}