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
                .HasMaxLength(100); 

            modelBuilder.Entity<Book>()
                .Property(t => t.Rate)
                .HasDefaultValue(0); 

            modelBuilder.Entity<Book>()
                .Property(t => t.ImageUrl)
                .HasDefaultValue("")
                .HasMaxLength(255);

            modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .HasMaxLength(50);
        }

        public DbSet<Book> Books {  get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ToReadList> ToReadList { get; set; }
    }
}
