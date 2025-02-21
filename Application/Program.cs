using System.CommandLine;
using DataAccess.Sqlite;
using Microsoft.EntityFrameworkCore;

TodoContext context = new();
try
{
    await context.Database.EnsureCreatedAsync();
    await context.Database.MigrateAsync();
}
catch
{
    // ignored
}

ITodoRepository repository = new TodoRepository(context);

Argument<string> headerArgument = new("header");
Argument<string> bodyArgument = new("body");
Argument<int> idArgument = new("id");

Argument<TodoStatus> statusArgument = new("status");

Command addCommand = new("add")
{
    headerArgument,
    bodyArgument
};

addCommand.SetHandler(async (h, b) =>
{
    Todo todo = new()
    {
        Header = h,
        Body = b
    };

    await repository.AddAsync(todo);

    Console.WriteLine(todo.ToString());
}, headerArgument, bodyArgument);

Command removeCommand = new("remove")
{
    idArgument
};

removeCommand.SetHandler(async id =>
{
    await repository.Remove(async query => await query.FirstOrDefaultAsync(todo => todo.Id == id));

    Console.WriteLine("Removed");
}, idArgument);

Command getAllCommand = new("all");
getAllCommand.SetHandler(async _ =>
{
    await foreach (Todo todo in context.Todos)
    {
        Console.WriteLine(todo.ToString());
    }
});

Command getByIdCommand = new("id")
{
    idArgument
};

getByIdCommand.SetHandler(async id =>
{
    Todo? todo = await context.Todos.FirstOrDefaultAsync(x => x.Id.Equals(id));

    Console.WriteLine(todo.ToString());
}, idArgument);


Command getByStatusCommand = new("status")
{
    statusArgument
};

getByStatusCommand.SetHandler(status =>
{
    foreach (Todo todo in context.Todos.Where(x => x.Status.Equals(status)))
    {
        Console.WriteLine(todo.ToString());
    }
}, statusArgument);

Command getCommand = new("get")
{
    getAllCommand,
    getByIdCommand,
    getByStatusCommand
};

Command updateCommand = new("update")
{
    idArgument,
    headerArgument,
    bodyArgument
};

updateCommand.SetHandler(async (id, header, body) =>
{
    Todo? todo = await context.Todos.FirstOrDefaultAsync(x => x.Id.Equals(id));
    todo.Header = header;
    todo.Body = body;
    context.Todos.Update(todo);
    await context.SaveChangesAsync();

    Console.WriteLine(todo.ToString());

}, idArgument, headerArgument, bodyArgument);

RootCommand rootCommand = new()
{
    addCommand,
    removeCommand,
    getCommand,
    updateCommand,
};

int invokeAsync = await rootCommand.InvokeAsync(args);
await context.DisposeAsync();
return invokeAsync;