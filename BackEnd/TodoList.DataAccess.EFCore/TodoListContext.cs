using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities;

namespace TodoList.DataAccess.EFCore
{
    public class TodoListContext : DbContext
    {
        public TodoListContext(DbContextOptions<TodoListContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .Property(u => u.Status)
                .HasConversion<string>()
                .HasMaxLength(50);
        }
        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
