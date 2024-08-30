using Microsoft.EntityFrameworkCore;
using MVCLearn.Models;

namespace MVCLearn.DataAcess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        { }

        DbSet<User> Users { get; set; }
        DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMessage>(entity =>
            {
                entity.HasOne(um => um.Message)
                      .WithMany(m => m.UserMessages)
                      .HasForeignKey(um => um.MessageId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(um => um.User)
                      .WithMany(m => m.UserMessages)
                      .HasForeignKey(um => um.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
