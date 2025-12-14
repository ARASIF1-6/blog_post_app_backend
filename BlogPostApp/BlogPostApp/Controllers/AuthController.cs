using BlogPostApp.Data;
using BlogPostApp.DTOs;
using BlogPostApp.Models;
using BlogPostApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogPostApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IJwtService _jwt;

        public AuthController(AppDbContext db, IPasswordHasher<User> passwordHasher, IJwtService jwt)
        {
            _db = db;
            _passwordHasher = passwordHasher;
            _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                return Conflict(new { message = "Email already registered." });

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                FullName = dto.FullName,
                Role = "User" // ⭐ Forced default role (secure option)
            };

            user.Password = _passwordHasher.HashPassword(user, dto.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var token = _jwt.GenerateToken(user);
            return Ok(new AuthResponseDto { Token = token, ExpiresAt = _jwt.GetExpiry() });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _db.Users.SingleOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return Unauthorized(new { message = "Invalid credentials." });

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Invalid credentials." });

            var token = _jwt.GenerateToken(user);
            return Ok(new AuthResponseDto { Token = token, ExpiresAt = _jwt.GetExpiry() });
        }
    }
}
