using Microsoft.AspNetCore.Mvc;
using dev_forum_api.Models;
using dev_forum_api.Interfaces;
using dev_forum_api.Services;
using dev_forum_api.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace dev_forum_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IJwtService _jwtService;
        private readonly AdminService _adminService;

        public UsersController(IUserRepository repo, IJwtService jwtService, AdminService adminService)
        {
            _repo = repo;
            _jwtService = jwtService;
            _adminService = adminService;
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
                Email = user.Email,
                Role = user.Role
            };
            return Ok(userDto);
        }

        // POST: api/users/register
        [HttpPost("register")]
        public async Task<ActionResult<LoginResponseDto>> Register([FromBody] UserRegisterDto dto)
        {
            // Check if user already exists by email and password
            var existingUser = (await _repo.GetAllAsync())
                .FirstOrDefault(u => u.Email == dto.Email && u.Password == dto.Password);

            if (existingUser != null)
            {
                // Return 409 Conflict if user already exists with same email and password
                return Conflict(new { message = "User already registered" });
            }

            // Create new user if not exists
            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password, // You should hash this in production!
                Role = AdminService.GetRoleForEmail(dto.Email) // Automatically assign role based on email
            };
            var created = await _repo.AddAsync(user);
            
            // Generate JWT token
            var token = _jwtService.GenerateToken(created);
            
            var userDto = new UserDto
            {
                Id = created.Id,
                Username = created.Username,
                Email = created.Email,
                Role = created.Role
            };
            
            var response = new LoginResponseDto
            {
                Token = token,
                User = userDto
            };
            
            return CreatedAtAction(nameof(GetUserByUsername), new { username = created.Username }, response);
        }



        // POST: api/users/login
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] UserLoginDto dto)
        {
            var users = await _repo.GetAllAsync();
            var user = users.FirstOrDefault(u =>
                u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase) &&
                u.Password == dto.Password);

            if (user == null)
                return Unauthorized(new { message = "Invalid credentials" });

            // Generate JWT token
            var token = _jwtService.GenerateToken(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };

            var response = new LoginResponseDto
            {
                Token = token,
                User = userDto
            };

            return Ok(response);
        }
    }
}