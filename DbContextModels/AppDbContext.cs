using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.DbContextModels
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }
    }

}
