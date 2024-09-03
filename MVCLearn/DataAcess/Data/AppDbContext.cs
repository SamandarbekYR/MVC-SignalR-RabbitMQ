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
    }
}
