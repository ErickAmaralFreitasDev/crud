using Microsoft.EntityFrameworkCore;
using TodoAPI.AppDataContext;
using TodoAPI.Models;
using TodoAPI.Middleware;
using TodoAPI.Services;
using TodoAPI.Interface;
// using Microsoft.Extensions.Options; 
// using Pomelo.EntityFrameworkCore.MySql.Infrastructure; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<DbSettings>(builder.Configuration.GetSection("DbSettings")); // Add this line

var connectionString = builder.Configuration.GetSection("DbSettings:ConnectionString").Value;
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // Add this line
builder.Services.AddProblemDetails();  // Add this line
// Adding of login 
builder.Services.AddLogging();  //  Add this line
builder.Services.AddScoped<ITodoServices, TodoServices>();

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
app.UseExceptionHandler();
app.UseAuthorization();
app.MapControllers();

app.Run();