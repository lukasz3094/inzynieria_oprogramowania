namespace Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;

    // Meeting created by the user
    public ICollection<Meeting> OrganizedMeetings { get; set; } = [];

    // Meetings the user has attended
    public ICollection<MeetingAttendee> MeetingsAttended { get; set; } = [];
}
