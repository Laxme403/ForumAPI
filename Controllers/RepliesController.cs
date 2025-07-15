using Microsoft.AspNetCore.Mvc;
using dev_forum_api.Interfaces;
using dev_forum_api.DTOs;
using dev_forum_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dev_forum_api.Controllers
{
    [ApiController]
    [Route("api/replies")]
    public class RepliesController : ControllerBase
    {
        private readonly IReplyRepository _repo;

        public RepliesController(IReplyRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reply>>> GetReplies()
        {
            var replies = await _repo.GetAllAsync();
            return Ok(replies);
        }

        [HttpGet("thread/{threadId}")]
        public async Task<ActionResult<IEnumerable<ReplyDto>>> GetRepliesForThread(int threadId)
        {
            var replies = await _repo.GetDtosByThreadIdAsync(threadId);
            return Ok(replies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reply>> GetReply(int id)
        {
            var replies = await _repo.GetAllAsync();
            var reply = replies.FirstOrDefault(r => r.Id == id);
            if (reply == null) return NotFound();
            return Ok(reply);
        }

        [HttpPost]
        public async Task<ActionResult<Reply>> CreateReply([FromBody] ReplyCreateDto dto)
        {
            var reply = new Reply
            {
                Content = dto.Content,
                ThreadId = dto.ThreadId,
                UserId = dto.UserId
            };
            var created = await _repo.AddAsync(reply);
            return CreatedAtAction(nameof(GetRepliesForThread), new { threadId = created.ThreadId }, created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReply(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }
    }
}
