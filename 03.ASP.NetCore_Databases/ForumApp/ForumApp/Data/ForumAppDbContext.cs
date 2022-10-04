using ForumApp.Data.Configure;
using ForumApp.Data.Models;
using Microsoft.EntityFrameworkCore;
using ForumApp.Models;

namespace ForumApp.Data
{
    public class ForumAppDbContext : DbContext
    {
        public ForumAppDbContext(DbContextOptions<ForumAppDbContext> options)
        : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration<Post>(new PostConfiguration());
            base.OnModelCreating(builder);
        }

        public DbSet<ForumApp.Models.PostViewModel> PostViewModel { get; set; }
    }
}
