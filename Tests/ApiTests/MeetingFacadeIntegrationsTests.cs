using Contracts.DTOs.Api;
using Domain.Entities;
using Domain.Enums;
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
    public async Task PlanMeeting_ReturnsTrue_WhenSlotIsAvailable_Standard()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = new User { Email = "standard@example.com", FullName = "Standard User" };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var facade = scope.ServiceProvider.GetRequiredService<IMeetingFacade>();

        var dto = new MeetingCreateDto
        {
            Title = "Standard Slot",
            Description = "Valid slot",
            StartTime = DateTime.Today.AddHours(14),
            EndTime = DateTime.Today.AddHours(15)
        };

        var result = await facade.PlanMeetingAsync(user.Id, dto, SchedulingStrategyType.Standard);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task PlanMeeting_ReturnsFalse_WhenSlotIsTaken_Standard()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = new User { Email = "conflict@example.com", FullName = "Conflict User" };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        db.Meetings.Add(new Meeting
        {
            Title = "Conflict",
            StartTime = DateTime.Today.AddHours(10),
            EndTime = DateTime.Today.AddHours(11),
            OrganizerId = user.Id
        });
        await db.SaveChangesAsync();

        var facade = scope.ServiceProvider.GetRequiredService<IMeetingFacade>();

        var dto = new MeetingCreateDto
        {
            Title = "Should Fail",
            Description = "Conflict test",
            StartTime = DateTime.Today.AddHours(10).AddMinutes(30),
            EndTime = DateTime.Today.AddHours(11).AddMinutes(30)
        };

        var result = await facade.PlanMeetingAsync(user.Id, dto, SchedulingStrategyType.Standard);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task PlanMeeting_ReturnsTrue_WhenNextSlotFound_Closest()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = new User { Email = "closest@example.com", FullName = "Closest User" };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        db.Meetings.Add(new Meeting
        {
            Title = "Early Block",
            StartTime = DateTime.Today.AddHours(9),
            EndTime = DateTime.Today.AddHours(10),
            OrganizerId = user.Id
        });
        await db.SaveChangesAsync();

        var facade = scope.ServiceProvider.GetRequiredService<IMeetingFacade>();

        var dto = new MeetingCreateDto
        {
            Title = "Try Closest Slot",
            Description = "Should shift",
            StartTime = DateTime.Today.AddHours(9),
            EndTime = DateTime.Today.AddHours(10)
        };

        var result = await facade.PlanMeetingAsync(user.Id, dto, SchedulingStrategyType.Closest);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task PlanMeeting_ReturnsFalse_WhenNoSlotFound_Closest()
    {
        using var scope = _factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = new User { Email = "full@example.com", FullName = "Fully Booked" };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        for (int i = 0; i < 9; i++)
        {
            db.Meetings.Add(new Meeting
            {
                Title = $"Block {i}",
                StartTime = DateTime.Today.AddHours(9 + i),
                EndTime = DateTime.Today.AddHours(10 + i),
                OrganizerId = user.Id
            });
        }
        await db.SaveChangesAsync();

        var facade = scope.ServiceProvider.GetRequiredService<IMeetingFacade>();

        var dto = new MeetingCreateDto
        {
            Title = "No Free Slot",
            Description = "Should return false",
            StartTime = DateTime.Today.AddHours(9),
            EndTime = DateTime.Today.AddHours(10)
        };

        var result = await facade.PlanMeetingAsync(user.Id, dto, SchedulingStrategyType.Closest);

        result.Should().BeFalse();
    }
}
