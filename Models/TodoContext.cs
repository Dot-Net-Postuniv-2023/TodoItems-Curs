using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace TodoApi.Models
{
    public class TodoContext : DbContext
    {
        private readonly IConfiguration configuration;

        public TodoContext(DbContextOptions<TodoContext> options, IConfiguration configuration)
            : base(options)
        {
            this.configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.configuration.GetConnectionString("TodoItemsConnection"));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
        }

        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DbSet<TodoSubItem> TodoSubItems { get; set; } = null!;

        public DbSet<User> ApplicationUsers { get; set; } = null!;
    }
}