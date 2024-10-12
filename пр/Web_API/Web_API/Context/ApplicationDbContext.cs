using Microsoft.EntityFrameworkCore;
using Web_API.Models;



namespace Web_API.Context
{
    public partial class ApplicationDbContext : DbContext
    {
        public DbSet<Hotel> Hotel { get; set; } = null!;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        } 
    }
}
