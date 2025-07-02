using Domain.Entities;

namespace Patterns.Builder;

public class MeetingBuilder : IMeetingBuilder
{
    private readonly Meeting _meeting = new();

    public IMeetingBuilder SetTitle(string title)
    {
        _meeting.Title = title;
        return this;
    }

    public IMeetingBuilder SetDescription(string? description)
    {
        _meeting.Description = description;
        return this;
    }

    public IMeetingBuilder SetStartTime(DateTime startTime)
    {
        _meeting.StartTime = startTime;
        return this;
    }

    public IMeetingBuilder SetEndTime(DateTime endTime)
    {
        _meeting.EndTime = endTime;
        return this;
    }

    public IMeetingBuilder SetLocation(string? location)
    {
        _meeting.Location = location;
        return this;
    }

    public IMeetingBuilder SetOrganizer(User organizer)
    {
        _meeting.Organizer = organizer;
        _meeting.OrganizerId = organizer.Id;
        return this;
    }

    public Meeting Build()
    {
        return _meeting;
    }
}
