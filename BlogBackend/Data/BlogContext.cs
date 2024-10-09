using Microsoft.EntityFrameworkCore;
using BlogBackend.Models;

namespace BlogBackend.Data
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; } // Add this line
    }
}
