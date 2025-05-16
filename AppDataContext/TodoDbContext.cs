using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using TodoAPI.Models;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace TodoAPI.AppDataContext
{

    // TodoDbContext class inherits from DbContext
     public class TodoDbContext : DbContext
     {
        private readonly DbSettings _dbsettings;

        public TodoDbContext(IOptions<DbSettings> dbSettings)
        {
            _dbsettings = dbSettings.Value;
        }

        public DbSet<Todo> Todos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(
                    _dbsettings.ConnectionString,
                    new MySqlServerVersion(new Version(8, 0, 42)),
                    mysqlOptions => mysqlOptions
                        .EnableRetryOnFailure()
                );
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Todo>()
                .ToTable("TodoAPI")
                .HasKey(x => x.Id);
            
            // Configurações adicionais específicas do MySQL podem ser adicionadas aqui
        }
    }
}