using BlogPostApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogPostApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<BlogPost> BlogPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasDefaultValue("User"); // ⭐ EF default value

            modelBuilder.Entity<BlogPost>()
            .Property(p => p.Id)
            .UseIdentityAlwaysColumn(); // Preferred for PostgreSQL IDENTITY

            base.OnModelCreating(modelBuilder);
        }
    }
}
