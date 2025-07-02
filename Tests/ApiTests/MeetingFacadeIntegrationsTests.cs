using Contracts.DTOs.Api;
using Domain.Entities;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Patterns.Facade;
using Xunit;

namespace ApiTests;

public class MeetingFacadeIntegrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
	private readonly CustomWebApplicationFactory _factory = factory;

	[Fact]
    public async Task PlanMeeting_ReturnsTrue_WhenSlotIsAvailable()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = new User { Email = "facadeuser@example.com", FullName = "Facade User" };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var userId = user.Id;

        var facade = scope.ServiceProvider.GetRequiredService<IMeetingFacade>();

        var dto = new MeetingCreateDto
        {
            Title = "Facade Test Meeting",
            Description = "Integration test via SQLite",
            StartTime = DateTime.Now.AddHours(1),
            EndTime = DateTime.Now.AddHours(2)
        };

        var result = await facade.PlanMeetingAsync(userId, dto);

        result.Should().BeTrue();
        var meetings = db.Meetings.Where(m => m.Title == dto.Title).ToList();
        meetings.Should().HaveCount(1);
        meetings.First().OrganizerId.Should().Be(userId);
    }

    [Fact]
    public async Task PlanMeeting_ReturnsFalse_WhenSlotIsTaken()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = new User { Email = "conflict@example.com", FullName = "Conflict User" };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var conflictMeeting = new Meeting
        {
            Title = "Conflict",
            StartTime = DateTime.Today.AddHours(10),
            EndTime = DateTime.Today.AddHours(11),
            OrganizerId = user.Id
        };
        db.Meetings.Add(conflictMeeting);
        await db.SaveChangesAsync();

        var facade = scope.ServiceProvider.GetRequiredService<IMeetingFacade>();

        var dto = new MeetingCreateDto
        {
            Title = "Overlapping Meeting",
            Description = "Should fail",
            StartTime = DateTime.Today.AddHours(10).AddMinutes(30),
            EndTime = DateTime.Today.AddHours(11).AddMinutes(30)
        };

        var result = await facade.PlanMeetingAsync(user.Id, dto);

        result.Should().BeFalse();
    }
}
