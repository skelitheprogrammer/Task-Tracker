using Microsoft.EntityFrameworkCore;
namespace DataAccess.Postgres;

public sealed class AppDBContext : DbContext
{
    public DbSet<Task> Tasks => Set<Task>();

    public AppDBContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Task.Config());
    }
}