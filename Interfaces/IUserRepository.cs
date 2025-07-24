using dev_forum_api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dev_forum_api.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByUsernameAsync(string username);
        Task<User> AddAsync(User user);
    }
}