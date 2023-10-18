using imageKeeper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace imageKeeper.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Friendship> Friendships { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.UserA)
                .WithMany(u => u.Friendships)
                .HasForeignKey(f => f.UserAId)
                .OnDelete(DeleteBehavior.Restrict); // Используйте желаемый способ удаления

            modelBuilder.Entity<Friendship>()
                .HasOne(f => f.UserB)
                .WithMany()
                .HasForeignKey(f => f.UserBId)
                .OnDelete(DeleteBehavior.Restrict); // Используйте желаемый способ удаления

            // Другие настройки модели данных

            base.OnModelCreating(modelBuilder);
        }

    }
}
