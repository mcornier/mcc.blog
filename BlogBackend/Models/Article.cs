using System;
using System.Collections.Generic;

namespace BlogBackend.Models
{
    public class Article
    {
        public string Id { get; set; } = Ulid.NewUlid().ToString();
        public string Author { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? ModifiedDate { get; set; }
        public string Language { get; set; } = string.Empty;
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
