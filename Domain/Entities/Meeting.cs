using Domain.Enums;

namespace Domain.Entities;

public class Meeting
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Location { get; set; }
    public MeetingStatus Status { get; set; } = MeetingStatus.Scheduled;

    // Organizer
    public int OrganizerId { get; set; }
    public User Organizer { get; set; } = null!;

    // Attendees
    public ICollection<MeetingAttendee> Attendees { get; set; } = new List<MeetingAttendee>();
}
