using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManagement.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Task",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "IsCompleted", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "this task was seeded via migration", true, false, "first seeded task", null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 2, 11, 0, 0, 0, DateTimeKind.Utc), "another seeded task", true, true, "second seeded task", null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 1, 3, 12, 0, 0, 0, DateTimeKind.Utc), "pending task for testing", true, false, "third seeded task", null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 1, 4, 13, 0, 0, 0, DateTimeKind.Utc), "this task is soft-deleted", false, false, "inactive seeded task", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Task",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Task",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Task",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Task",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));
        }
    }
}
