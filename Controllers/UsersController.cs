using Microsoft.AspNetCore.Mvc;
using dev_forum_api.Models;
using dev_forum_api.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dev_forum_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UsersController(IUserRepository repo)
        {
            _repo = repo;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var users = await _repo.GetAllAsync();
            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email
            });
            return Ok(userDtos);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();
            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
            return Ok(userDto);
        }

        // GET: api/users/by-username/{username}
        [HttpGet("by-username/{username}")]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            var user = await _repo.GetByUsernameAsync(username);
            if (user == null)
                return NotFound(new { message = "User not found" });

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
            return Ok(userDto);
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserRegisterDto dto)
        {
            // Check if user already exists by email or username
            var existingUser = (await _repo.GetAllAsync())
                .FirstOrDefault(u => u.Email == dto.Email || u.Username == dto.Username);

            if (existingUser != null)
            {
                // Return the existing user's info (or an error if you prefer)
                var userDto = new UserDto
                {
                    Id = existingUser.Id,
                    Username = existingUser.Username,
                    Email = existingUser.Email
                };
                return Ok(userDto); // Or: return BadRequest("User already exists");
            }

            // Create new user if not exists
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password // You should hash this in production!
            };
            var created = await _repo.AddAsync(user);
            var createdDto = new UserDto
            {
                Id = created.Id,
                Username = created.Username,
                Email = created.Email
            };
            return CreatedAtAction(nameof(GetUser), new { id = created.Id }, createdDto);
        }

        // PUT: api/users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserRegisterDto dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();

            user.Username = dto.Username;
            user.Email = dto.Email;
            user.Password = dto.Password;

            await _repo.UpdateAsync(user);
            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _repo.DeleteAsync(id);
            return NoContent();
        }

        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] UserLoginDto dto)
        {
            var user = (await _repo.GetAllAsync())
                .FirstOrDefault(u =>
                    u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase) && // case-insensitive email
                    u.Password == dto.Password); // password remains case-sensitive

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
            return Ok(userDto);
        }
    }
}