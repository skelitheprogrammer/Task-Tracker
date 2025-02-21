namespace DataAccess.Sqlite;

public interface ITodoRepository
{
    Task AddAsync(Todo task);

    IAsyncEnumerable<Todo> Get(Func<IEnumerable<Todo>, IQueryable<Todo>> query);
    Task Update(Func<IEnumerable<Todo>, IQueryable<Todo>> query, string body, string header);
    Task Remove(Func<IQueryable<Todo>, Task<Todo?>> query);
    Task RemoveRange(Func<IEnumerable<Todo>, IQueryable<Todo>> query);
}