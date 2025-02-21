using Microsoft.EntityFrameworkCore;
namespace DataAccess.Sqlite;

public sealed class TodoRepository : ITodoRepository
{
    private readonly TodoContext _context;

    public TodoRepository(TodoContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Todo task)
    {
        await _context.Todos.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public IAsyncEnumerable<Todo> Get(Func<IEnumerable<Todo>, IQueryable<Todo>> query) => query(_context.Todos).AsAsyncEnumerable();

    public async Task Update(Func<IEnumerable<Todo>, IQueryable<Todo>> query, string body, string header) => await query(_context.Todos).ExecuteUpdateAsync(s => s
        .SetProperty(b => b.Body, b => body)
        .SetProperty(h => h.Header, h => header));

    public async Task Remove(Func<IQueryable<Todo>, Task<Todo?>> query)
    {
        Todo todo = await query(_context.Todos);

        if (todo is null)
        {
            throw new ArgumentException("Todo with this requirements does not exists");
        }

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveRange(Func<IEnumerable<Todo>, IQueryable<Todo>> query) => await query(_context.Todos).ExecuteDeleteAsync();
}