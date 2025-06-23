using Microsoft.AspNetCore.Mvc;
using dev_forum_api.Models;
using dev_forum_api.Data;
using Microsoft.EntityFrameworkCore;

namespace dev_forum_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThreadsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ThreadsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/threads
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllThreads()
        {
            // Step 1: Fetch tags as string
            var threads = await _context.Threads
                .Select(t => new {
                    id = t.Id,
                    title = t.Title,
                    description = t.Description,
                    userId = t.UserId,
                    author = t.Author,
                    likes = t.Likes,
                    dislikes = t.Dislikes,
                    tags = t.Tags, // just as string
                    replies = _context.Replies.Count(r => r.ThreadId == t.Id)
                })
                .ToListAsync();

            // Step 2: Split tags in memory
            var result = threads.Select(t => new {
                t.id,
                t.title,
                t.description,
                t.userId,
                t.author,
                t.likes,
                t.dislikes,
                tags = string.IsNullOrEmpty(t.tags)
                    ? new string[0]
                    : t.tags.Split(',').Select(tag => tag.Trim()).ToArray(),
                t.replies
            });

            return Ok(result);
        }

        // GET: api/threads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetThreadById(int id)
        {
            var thread = await _context.Threads
                .FirstOrDefaultAsync(t => t.Id == id);

            if (thread == null) return NotFound();

            // Fetch the user (author) from Users table
            var user = await _context.Users.FindAsync(thread.UserId);

            // Fetch all replies for this thread, including author info
            var replies = await _context.Replies
                .Where(r => r.ThreadId == thread.Id)
                .Select(r => new {
                    id = r.Id,
                    content = r.Content,
                    userId = r.UserId,
                    threadid = r.ThreadId,
                    author = r.Author// Only the username as a string
                })
                .ToListAsync();

            return Ok(new {
                id = thread.Id,
                title = thread.Title,
                description = thread.Description,
                userId = thread.UserId,
                author = user == null ? null : new { id = user.Id, username = user.Username },
                replies = replies, // full list of replies
                tags = thread.Tags?.Split(',').Select(t => t.Trim()).ToArray() ?? new string[0],
                likes = thread.Likes,
                dislikes = thread.Dislikes
            });
        }

        // POST: api/threads
        [HttpPost]
        public async Task<ActionResult<ForumThread>> CreateThread([FromBody] ForumThread newThread)
        {
            _context.Threads.Add(newThread);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetThreadById), new { id = newThread.Id }, newThread);
        }

        // PUT: api/threads/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateThread(int id, [FromBody] ForumThread updatedThread)
        {
            if (id != updatedThread.Id)
                return BadRequest();

            _context.Entry(updatedThread).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Threads.Any(t => t.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/threads/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteThread(int id)
        {
            var thread = await _context.Threads.FindAsync(id);
            if (thread == null)
                return NotFound();

            _context.Threads.Remove(thread);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
