using Microsoft.EntityFrameworkCore;
using Explorer.Blog.Core.Domain;


namespace Explorer.Blog.Infrastructure.Database;

public class BlogContext : DbContext
{
    public DbSet<Core.Domain.Blog> Blogs { get; set; }
    public DbSet<Core.Domain.Comment> Comments { get; set; }    
    public BlogContext(DbContextOptions<BlogContext> options) : base(options) {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("blog");
        modelBuilder.Entity<Core.Domain.Blog>().
            HasMany(b => b.Comments).WithOne();

        modelBuilder.Entity<Core.Domain.Blog>().
            Property(b => b.Ratings).HasColumnType("jsonb");
    }
}