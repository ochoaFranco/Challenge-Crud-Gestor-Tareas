using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain;

namespace TaskManagement.Infraestructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) {}
        public DbSet<TaskItem> Tasks { get; set; }
    }
}
