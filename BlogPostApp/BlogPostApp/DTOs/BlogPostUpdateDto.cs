namespace BlogPostApp.DTOs
{
    public class BlogPostUpdateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile? Image { get; set; } // optional
    }
}
