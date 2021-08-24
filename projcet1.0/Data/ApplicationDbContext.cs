using projcet1._0.Models;
using Microsoft.EntityFrameworkCore;
using project1._0.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace projcet1._0.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ArticleLike>()
              .HasKey(x => new { x.LikeID, x.ArticleID });
        }

        public DbSet<projcet1._0.Models.Map> Map { get; set; }

        public DbSet<Like> Likes { get; set; }

        public DbSet<projcet1._0.Models.ArticleLike> ArticleLike { get; set; }

    }
}
