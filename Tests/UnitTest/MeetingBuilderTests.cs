using System;
using Domain.Entities;
using FluentAssertions;
using Patterns.Builder;
using Xunit;

namespace UnitTest;

public class MeetingBuilderTests
{
    [Fact]
    public void Build_Should_CreateMeetingWithAllFields()
    {
        var organizer = new User { Id = 1, Email = "test@example.com", FullName = "Test User" };
        var builder = new MeetingBuilder();

        var meeting = builder
            .SetTitle("Test Meeting")
            .SetDescription("Builder pattern test")
            .SetStartTime(DateTime.Today.AddHours(10))
            .SetEndTime(DateTime.Today.AddHours(11))
            .SetLocation("Room A")
            .SetOrganizer(organizer)
            .Build();

        meeting.Title.Should().Be("Test Meeting");
        meeting.Description.Should().Be("Builder pattern test");
        meeting.StartTime.Should().Be(DateTime.Today.AddHours(10));
        meeting.EndTime.Should().Be(DateTime.Today.AddHours(11));
        meeting.Location.Should().Be("Room A");
        meeting.OrganizerId.Should().Be(organizer.Id);
        meeting.Organizer.Should().Be(organizer);
    }

    [Fact]
    public void Build_Should_AllowOptionalFieldsToBeOmitted()
    {
        var organizer = new User { Id = 2, Email = "minimal@example.com", FullName = "Minimal User" };
        var builder = new MeetingBuilder();

        var meeting = builder
            .SetTitle("Quick")
            .SetStartTime(DateTime.Now)
            .SetEndTime(DateTime.Now.AddMinutes(30))
            .SetOrganizer(organizer)
            .Build();

        meeting.Title.Should().Be("Quick");
        meeting.Description.Should().BeNull();
        meeting.Location.Should().BeNull();
    }
}
