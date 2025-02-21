using Microsoft.EntityFrameworkCore;
namespace DataAccess.Sqlite;

public sealed class TodoContext : DbContext
{
    public DbSet<Todo> Todos => Set<Todo>();

    private readonly string DbPath;

    public TodoContext()
    {
        const Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
        string path = Environment.GetFolderPath(folder) + @"\todoApp";
        Directory.CreateDirectory(path);
        DbPath = Path.Join(path, "todos.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder) => modelBuilder.ApplyConfiguration(new Todo.Config());
}