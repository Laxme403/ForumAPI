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

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] UserRegisterDto dto)
        {
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password
            };
            var created = await _repo.AddAsync(user);
            var userDto = new UserDto
            {
                Id = created.Id,
                Username = created.Username,
                Email = created.Email
            };
            return CreatedAtAction(nameof(GetUser), new { id = created.Id }, userDto);
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
    }
}