using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infraestructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) {}
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TaskItem>().HasData(
                 new TaskItem
                 {
                     Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                     Title = "first seeded task",
                     Description = "this task was seeded via migration",
                     IsCompleted = false,
                     IsActive = true,
                     CreatedAt = new DateTime(2026, 01, 01, 10, 00, 00, DateTimeKind.Utc)
                 },
                new TaskItem
                {
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Title = "second seeded task",
                    Description = "another seeded task",
                    IsCompleted = true,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 01, 02, 11, 00, 00, DateTimeKind.Utc)
                },
                new TaskItem
                {
                    Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    Title = "third seeded task",
                    Description = "pending task for testing",
                    IsCompleted = false,
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 01, 03, 12, 00, 00, DateTimeKind.Utc)
                },
                new TaskItem
                {
                    Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
                    Title = "inactive seeded task",
                    Description = "this task is soft-deleted",
                    IsCompleted = false,
                    IsActive = false,
                    CreatedAt = new DateTime(2026, 01, 04, 13, 00, 00, DateTimeKind.Utc)
                });
        }
    }
}