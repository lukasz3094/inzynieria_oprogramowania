using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<Meeting> Meetings => Set<Meeting>();
	public DbSet<User> Users => Set<User>();
	public DbSet<MeetingAttendee> MeetingAttendees => Set<MeetingAttendee>();
	
	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Meeting>()
            .HasOne(m => m.Organizer)
            .WithMany(u => u.OrganizedMeetings)
            .HasForeignKey(m => m.OrganizerId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MeetingAttendee>()
            .HasOne(ma => ma.Meeting)
            .WithMany(m => m.Attendees)
            .HasForeignKey(ma => ma.MeetingId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MeetingAttendee>()
            .HasOne(ma => ma.User)
            .WithMany(u => u.MeetingsAttended)
            .HasForeignKey(ma => ma.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MeetingAttendee>()
            .HasIndex(ma => new { ma.MeetingId, ma.UserId })
            .IsUnique();
    }
}
