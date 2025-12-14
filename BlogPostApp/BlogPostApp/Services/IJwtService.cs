using BlogPostApp.Models;

namespace BlogPostApp.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        DateTime GetExpiry();
    }
}
