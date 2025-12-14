using BlogPostApp.Data;
using BlogPostApp.DTOs;
using BlogPostApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogPostApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]   // 🔒 Protect entire controller
    public class BlogPostController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _env;

        public BlogPostController(AppDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // ----------------------------------------------
        // CREATE POST
        // ----------------------------------------------
        [Authorize] // Specific endpoint secure
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] BlogPostCreateDto dto)
        {
            if (dto.Image == null || dto.Image.Length == 0)
                return BadRequest("Image is required.");

            // Create uploads folder if not exists
            string uploadFolder = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            // Unique file name
            string fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
            string filePath = Path.Combine(uploadFolder, fileName);

            // Save image
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(stream);
            }

            var post = new BlogPost
            {
                Title = dto.Title,
                Content = dto.Content,
                ImageUrl = "/uploads/" + fileName,
            };

            await _db.BlogPosts.AddAsync(post);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                message = "Post created successfully",
                data = post
            });
        }

        // ----------------------------------------------
        // GET ALL POSTS
        // ----------------------------------------------
        [AllowAnonymous] // Endpoint publicly access without any authrization
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var posts = await _db.BlogPosts.OrderByDescending(p => p.CreatedAt).ToListAsync();
            return Ok(posts);
        }

        // ----------------------------------------------
        // GET SINGLE POST BY ID
        // ----------------------------------------------
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var post = await _db.BlogPosts.FindAsync(id);
            if (post == null)
                return NotFound("Post not found");

            return Ok(post);
        }

        // ----------------------------------------------
        // UPDATE POST (WITH OPTIONAL IMAGE)
        // ----------------------------------------------
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] BlogPostUpdateDto dto)
        {
            var post = await _db.BlogPosts.FindAsync(id);
            if (post == null)
                return NotFound("Post not found");

            post.Title = dto.Title;
            post.Content = dto.Content;

            // If image uploaded → replace old image
            if (dto.Image != null)
            {
                string uploadFolder = Path.Combine(_env.ContentRootPath, "uploads");
                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                // delete old image if exists
                if (!string.IsNullOrEmpty(post.ImageUrl))
                {
                    string oldPath = Path.Combine(_env.ContentRootPath, post.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                // save new image
                string fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                    await dto.Image.CopyToAsync(stream);

                post.ImageUrl = "/uploads/" + fileName;
            }

            await _db.SaveChangesAsync();
            return Ok(new { message = "Post updated successfully", post });
        }

        // ----------------------------------------------
        // DELETE POST
        // ----------------------------------------------
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _db.BlogPosts.FindAsync(id);
            if (post == null)
                return NotFound("Post not found");

            // delete image file
            if (!string.IsNullOrEmpty(post.ImageUrl))
            {
                string imagePath = Path.Combine(_env.ContentRootPath, post.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _db.BlogPosts.Remove(post);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Post deleted successfully" });
        }

        // ----------------------------------------------
        // SERVE IMAGES
        // ----------------------------------------------
        [AllowAnonymous]
        [HttpGet("image/{fileName}")]
        public IActionResult GetImage(string fileName)
        {
            string filePath = Path.Combine(_env.ContentRootPath, "uploads", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Image not found");

            var mimeType = "image/" + Path.GetExtension(fileName).TrimStart('.');
            return PhysicalFile(filePath, mimeType);
        }
    }
}
