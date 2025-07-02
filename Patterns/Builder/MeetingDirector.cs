using Domain.Entities;

namespace Patterns.Builder;

public class MeetingDirector(IMeetingBuilder builder)
{
    private readonly IMeetingBuilder _builder = builder;

	public Meeting CreateBasicMeeting(string title, DateTime start, DateTime end, User organizer)
    {
        return _builder
            .SetTitle(title)
            .SetStartTime(start)
            .SetEndTime(end)
            .SetOrganizer(organizer)
            .Build();
    }
}
