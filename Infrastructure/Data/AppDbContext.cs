using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
	// protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	// {
	// 	optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.CommandExecuting));
	// }

    public AppDbContext(DbContextOptions<AppDbContext> options)
		: base(options)
	{
	}

    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<User> Users => Set<User>();
    public DbSet<MeetingAttendee> MeetingAttendees => Set<MeetingAttendee>();
}
