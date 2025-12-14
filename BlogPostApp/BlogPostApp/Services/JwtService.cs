using BlogPostApp.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlogPostApp.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettings _settings;
        private DateTime? _expiry;

        public JwtService(IOptions<JwtSettings> options)
        {
            _settings = options.Value;
        }

        public string GenerateToken(User user)
        {
            var keyBytes = Encoding.UTF8.GetBytes(_settings.Key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            _expiry = DateTime.UtcNow.AddMinutes(_settings.ExpireMinutes);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("fullname", user.FullName ?? string.Empty),
            new Claim(ClaimTypes.Role, user.Role) // ⭐ Important
        };

            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires: _expiry,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public DateTime GetExpiry()
        {
            return _expiry ?? DateTime.UtcNow.AddMinutes(_settings.ExpireMinutes);
        }
    }
}
