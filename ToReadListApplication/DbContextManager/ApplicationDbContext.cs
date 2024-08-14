using Microsoft.EntityFrameworkCore;
using ToReadListApplication.Models;

namespace ToReadListApplication.DbContextManager
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>().
                Property(t => t.Description)
                .IsRequired(false)
                .HasMaxLength(150);

            modelBuilder.Entity<Book>()
                .Property(t => t.Author)
                .IsRequired()
                .HasMaxLength(100); 

            modelBuilder.Entity<Book>()
                .Property(t => t.Rate)
                .IsRequired()
                .HasDefaultValue(0); 

            modelBuilder.Entity<Book>()
                .Property(t => t.ImageUrl)
                .IsRequired()
                .HasMaxLength(255); 

            modelBuilder.Entity<Book>()
                .Property(t => t.PublishDate)
                .IsRequired();

            modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(50);
        }

        public DbSet<Book> Books {  get; set; }
        public DbSet<Category> Categories { get; set; }

    

    }
}
