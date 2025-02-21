using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskStatus = Domain.TaskStatus;

namespace DataAccess.Postgres;

public sealed class Task
{
    public Guid Id { get; init; }

    public string Title { get; set; }

    public string Description { get; set; }

    public TaskStatus Status { get; set; }

    internal sealed class Config : IEntityTypeConfiguration<Task>
    {

        public void Configure(EntityTypeBuilder<Task> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Status).HasDefaultValue(TaskStatus.BACKLOG);
        }
    }
}