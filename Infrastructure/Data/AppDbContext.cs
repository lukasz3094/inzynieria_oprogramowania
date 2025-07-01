using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<User> Users => Set<User>();
    public DbSet<MeetingAttendee> MeetingAttendees => Set<MeetingAttendee>();
}
