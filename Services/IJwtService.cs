using dev_forum_api.Models;

namespace dev_forum_api.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        bool ValidateToken(string token);
        int? GetUserIdFromToken(string token);
    }
}
