namespace Api.DTOs.Api;

public class MeetingCreateDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Location { get; set; }
    public int OrganizerId { get; set; }
}
