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
        public async Task<ActionResult<IEnumerable<ForumThread>>> GetAllThreads()
        {
            return await _context.Threads.ToListAsync();
        }

        // GET: api/threads/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ForumThread>> GetThreadById(int id)
        {
            var thread = await _context.Threads.FindAsync(id);
            return thread == null ? NotFound() : Ok(thread);
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
