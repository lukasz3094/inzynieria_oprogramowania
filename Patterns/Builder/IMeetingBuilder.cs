using Domain.Entities;

namespace Patterns.Builder;

public interface IMeetingBuilder
{
    IMeetingBuilder SetTitle(string title);
    IMeetingBuilder SetDescription(string? description);
    IMeetingBuilder SetStartTime(DateTime startTime);
    IMeetingBuilder SetEndTime(DateTime endTime);
    IMeetingBuilder SetLocation(string? location);
    IMeetingBuilder SetOrganizer(User organizer);
    Meeting Build();
}
