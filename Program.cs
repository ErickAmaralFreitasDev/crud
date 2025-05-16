using Microsoft.EntityFrameworkCore;
using TodoAPI.AppDataContext;
using TodoAPI.Models;
using AutoMapper;
// using Microsoft.Extensions.Options; 
// using Pomelo.EntityFrameworkCore.MySql.Infrastructure; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings")); // Add this line

builder.Services.AddDbContext<TodoDbContext>(options => 
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 42))
    ));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); 

var app = builder.Build();

{
    using var scope = app.Services.CreateScope(); // Add this line
    var context = scope.ServiceProvider; // Add this line
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();