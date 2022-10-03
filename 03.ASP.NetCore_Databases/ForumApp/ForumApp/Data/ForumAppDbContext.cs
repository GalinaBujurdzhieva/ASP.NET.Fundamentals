using ForumApp.Data.Configure;
using ForumApp.Data.Models;
using Microsoft.EntityFrameworkCore;

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
    }
}
