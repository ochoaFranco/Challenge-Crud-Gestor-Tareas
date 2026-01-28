using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Application.Services;
using TaskManagement.Infraestructure.Persistence;
using TaskManagement.Infraestructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddTransient<ITaskService, TaskService>();
builder.Services.AddTransient<ITaskRepository, TaskRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
