using Microsoft.AspNetCore.Mvc;
using dev_forum_api.Models;
using dev_forum_api.Data;
using Microsoft.EntityFrameworkCore;

namespace dev_forum_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RepliesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RepliesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/replies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reply>>> GetReplies()
        {
            return await _context.Replies.ToListAsync();
        }

        // GET: api/replies/thread/3 (get replies by thread id)
        [HttpGet("thread/{threadId}")]
        public async Task<ActionResult<IEnumerable<Reply>>> GetRepliesForThread(int threadId)
        {
            return await _context.Replies.Where(r => r.ThreadId == threadId).ToListAsync();
        }

        // GET: api/replies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reply>> GetReply(int id)
        {
            var reply = await _context.Replies.FindAsync(id);
            if (reply == null) return NotFound();
            return reply;
        }

        // POST: api/replies
        [HttpPost]
        public async Task<ActionResult<Reply>> CreateReply(Reply reply)
        {
            _context.Replies.Add(reply);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetReply), new { id = reply.Id }, reply);
        }

        // PUT: api/replies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReply(int id, Reply reply)
        {
            if (id != reply.Id) return BadRequest();

            _context.Entry(reply).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Replies.Any(e => e.Id == id)) return NotFound();
                else throw;
            }

            return NoContent();
        }

        // DELETE: api/replies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReply(int id)
        {
            var reply = await _context.Replies.FindAsync(id);
            if (reply == null) return NotFound();

            _context.Replies.Remove(reply);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}