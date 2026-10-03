using _20_VuNgocQuan_Assignment01.Models;
using Microsoft.EntityFrameworkCore;

namespace _20_VuNgocQuan_Assignment01.DAOs;

public class FUNewsManagementDbContext : DbContext
{
    public FUNewsManagementDbContext(DbContextOptions<FUNewsManagementDbContext> options)
        : base(options) { }

    public DbSet<SystemAccount> SystemAccounts { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<NewsArticle> NewsArticles { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<NewsTag> NewsTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Composite PK for NewsTag
        modelBuilder.Entity<NewsTag>()
            .HasKey(nt => new { nt.NewsArticleID, nt.TagID });

        // Category self-referencing (disable cascade to avoid cycles)
        modelBuilder.Entity<Category>()
            .HasOne(c => c.ParentCategory)
            .WithMany(c => c.SubCategories)
            .HasForeignKey(c => c.ParentCategoryID)
            .OnDelete(DeleteBehavior.NoAction);

        // NewsArticle -> Category (cascade per DB schema)
        modelBuilder.Entity<NewsArticle>()
            .HasOne(n => n.Category)
            .WithMany(c => c.NewsArticles)
            .HasForeignKey(n => n.CategoryID)
            .OnDelete(DeleteBehavior.Cascade);

        // NewsArticle -> SystemAccount (cascade per DB schema)
        modelBuilder.Entity<NewsArticle>()
            .HasOne(n => n.CreatedBy)
            .WithMany(a => a.CreatedArticles)
            .HasForeignKey(n => n.CreatedByID)
            .OnDelete(DeleteBehavior.Cascade);

        // NewsTag -> NewsArticle
        modelBuilder.Entity<NewsTag>()
            .HasOne(nt => nt.NewsArticle)
            .WithMany(n => n.NewsTags)
            .HasForeignKey(nt => nt.NewsArticleID)
            .OnDelete(DeleteBehavior.Cascade);

        // NewsTag -> Tag
        modelBuilder.Entity<NewsTag>()
            .HasOne(nt => nt.Tag)
            .WithMany(t => t.NewsTags)
            .HasForeignKey(nt => nt.TagID)
            .OnDelete(DeleteBehavior.NoAction);

        // SystemAccount PK is not identity (manually assigned)
        modelBuilder.Entity<SystemAccount>()
            .Property(a => a.AccountID)
            .ValueGeneratedNever();
    }
}
