using System;

namespace BlogBackend.Models
{
    public class Comment
    {
        public string Id { get; set; } = Ulid.NewUlid().ToString();
        public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string ArticleId { get; set; } = string.Empty;
    }
}
