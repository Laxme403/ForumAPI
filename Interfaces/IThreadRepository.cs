using dev_forum_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dev_forum_api.Interfaces
{
    public interface IThreadRepository
    {
        Task<IEnumerable<ForumThread>> GetAllAsync();
        Task<ForumThread> GetByIdAsync(int id);
        Task<ForumThread> AddAsync(ForumThread thread);
        Task UpdateAsync(ForumThread thread);
        Task DeleteAsync(int id);

        // Add these methods:
        Task<List<ForumThread>> GetThreadsLikedByUser(int userId);
        Task<List<ForumThread>> GetThreadsDislikedByUser(int userId);
        Task<(int likes, int dislikes)> LikeThread(int threadId, int userId);
        Task<(int likes, int dislikes)> DislikeThread(int threadId, int userId);
    }
}