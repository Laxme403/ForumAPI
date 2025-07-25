using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using dev_forum_api.Models;
using dev_forum_api.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dev_forum_api.Controllers
{
    [ApiController]
    [Route("api/threads")]
    public class ThreadController : ControllerBase
    {
        private readonly IThreadRepository _repo;

        public ThreadController(IThreadRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ThreadDto>>> GetThreads()
        {
            var threads = await _repo.GetAllAsync();
            var threadDtos = threads
                .Where(t => t.deleteindex == 0)
                .Select(t => new ThreadDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    UserId = t.UserId,
                    Author = t.Author,
                    Likes = t.Likes,
                    Dislikes = t.Dislikes,
                    Tags = t.Tags,
                    Replies = t.Replies,
                    deleteindex = t.deleteindex,
                    CreatedAt = t.CreatedAt
                });
            return Ok(threadDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ThreadDto>> GetThread(int id)
        {
            var thread = await _repo.GetByIdAsync(id);
            if (thread == null) return NotFound();
            var threadDto = new ThreadDto
            {
                Id = thread.Id,
                Title = thread.Title,
                Description = thread.Description,
                UserId = thread.UserId,
                Author = thread.Author,
                Likes = thread.Likes,
                Dislikes = thread.Dislikes,
                Tags = thread.Tags,
                Replies = thread.Replies
            };
            return Ok(threadDto);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ThreadDto>> CreateThread([FromBody] ThreadCreateDto dto)
        {
            var thread = new ForumThread
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = dto.UserId,
                Author = dto.Author,
                Tags = dto.Tags,
                Likes = 0,
                Dislikes = 0,
                Replies = 0,
                deleteindex = 0,
                CreatedAt = DateTime.UtcNow
            };
            var created = await _repo.AddAsync(thread);
            var threadDto = new ThreadDto
            {
                Id = created.Id,
                Title = created.Title,
                Description = created.Description,
                UserId = created.UserId,
                Author = created.Author,
                Likes = created.Likes,
                Dislikes = created.Dislikes,
                Tags = created.Tags,
                Replies = created.Replies,
                deleteindex = created.deleteindex
            };
            return CreatedAtAction(nameof(GetThread), new { id = created.Id }, threadDto);
        }



        // SOFT DELETE: Only this endpoint remains
        [HttpPut("{id}/soft-delete")]
        [Authorize]
        public async Task<IActionResult> SoftDeleteThread(int id, [FromBody] SoftDeleteDto dto)
        {
            var thread = await _repo.GetByIdAsync(id);
            if (thread == null) return NotFound();

            thread.deleteindex = dto.deleteindex;
            await _repo.UpdateAsync(thread);
            return NoContent();
        }

        [HttpGet("liked/{userId}")]
        public async Task<ActionResult<IEnumerable<ThreadDto>>> GetThreadsLikedByUser(int userId)
        {
            var threads = await _repo.GetThreadsLikedByUser(userId);
            var threadDtos = threads.Select(t => new ThreadDto {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                UserId = t.UserId,
                Author = t.Author,
                Likes = t.Likes,
                Dislikes = t.Dislikes,
                Tags = t.Tags,
                Replies = t.Replies,
                deleteindex = t.deleteindex,
                CreatedAt = t.CreatedAt
            });
            return Ok(threadDtos);
        }

        [HttpGet("disliked/{userId}")]
        public async Task<ActionResult<IEnumerable<ThreadDto>>> GetThreadsDislikedByUser(int userId)
        {
            var threads = await _repo.GetThreadsDislikedByUser(userId);
            var threadDtos = threads.Select(t => new ThreadDto {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                UserId = t.UserId,
                Author = t.Author,
                Likes = t.Likes,
                Dislikes = t.Dislikes,
                Tags = t.Tags,
                Replies = t.Replies,
                deleteindex = t.deleteindex,
                CreatedAt = t.CreatedAt
            });
            return Ok(threadDtos);
        }

        [HttpPost("{threadId}/like")]
        [Authorize]
        public async Task<IActionResult> LikeThread(int threadId, [FromBody] UserIdDto dto)
        {
            var (likes, dislikes) = await _repo.LikeThread(threadId, dto.UserId);
            return Ok(new { likes, dislikes });
        }

        [HttpPost("{threadId}/dislike")]
        [Authorize]
        public async Task<IActionResult> DislikeThread(int threadId, [FromBody] UserIdDto dto)
        {
            var (likes, dislikes) = await _repo.DislikeThread(threadId, dto.UserId);
            return Ok(new { likes, dislikes });
        }
    }
}