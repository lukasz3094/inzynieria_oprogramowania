namespace Contracts.DTOs.Api;

public class MeetingDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Location { get; set; }
    public int OrganizerId { get; set; }
    public string OrganizerName { get; set; } = string.Empty;
    public List<UserDetailsDto> Attendees { get; set; } = [];
}
