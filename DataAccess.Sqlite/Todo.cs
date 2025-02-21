using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataAccess.Sqlite;

public enum TodoStatus
{
    BACKLOG,
    IN_PROGRESS,
    DONE
}
public class Todo
{
    public int Id { get; set; }

    public string Header { get; set; }

    public string Body { get; set; }

    public TodoStatus Status { get; set; }

    internal sealed class Config : IEntityTypeConfiguration<Todo>
    {

        public void Configure(EntityTypeBuilder<Todo> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status).HasDefaultValue(TodoStatus.BACKLOG);
        }
    }

    public override string ToString() => $"{Id} {Header} {Body}";
}