namespace Domain.Entities;

public class MeetingAttendee
{
	public int Id { get; set; }
    public int MeetingId { get; set; }
    public Meeting Meeting { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
