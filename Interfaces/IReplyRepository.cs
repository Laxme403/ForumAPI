using dev_forum_api.DTOs;
using dev_forum_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dev_forum_api.Interfaces
{
    public interface IReplyRepository
    {
        Task<Reply> AddAsync(Reply reply);
        Task<IEnumerable<Reply>> GetByThreadIdAsync(int threadId);
        Task<IEnumerable<Reply>> GetAllAsync();
        Task DeleteAsync(int id);

        // Add this for richer API responses
        Task<IEnumerable<ReplyDto>> GetDtosByThreadIdAsync(int threadId);
    }
}