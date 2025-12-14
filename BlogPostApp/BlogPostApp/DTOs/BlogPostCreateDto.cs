namespace BlogPostApp.DTOs
{
    public class BlogPostCreateDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }
    }
}
