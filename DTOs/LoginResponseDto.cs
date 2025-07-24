using dev_forum_api.Models;

namespace dev_forum_api.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new UserDto();
    }
}
