using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogPostApp.Models
{
    [Table("APP_USERS")]
    public class User
    {
        [Key]
        [Column("ID")]
        public Guid Id { get; set; }

        [Required]
        [Column("EMAIL")]
        [MaxLength(256)]
        public string Email { get; set; }

        [Required]
        [Column("PASSWORD")]
        public string Password { get; set; }

        [Column("FULLNAME")]
        [MaxLength(200)]
        public string? FullName { get; set; }

        [Column("ROLE")]
        [MaxLength(50)]
        public string Role { get; set; } = "User";  // ⭐ Default value

        [Column("CREATED_AT")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
