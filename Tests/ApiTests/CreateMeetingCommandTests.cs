using Contracts.DTOs.Api;
using Domain.Entities;
using Domain.Interfaces;
using FluentAssertions;
using Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Patterns.Command;
using Xunit;

namespace ApiTests;

public class CreateMeetingCommandTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task Execute_Should_Create_Meeting_In_Db()
    {
        using var scope = factory.Services.CreateScope();
		var userRepo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
		var meetingRepo = scope.ServiceProvider.GetRequiredService<IMeetingRepository>();

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var user = new User { Email = "commandtest@example.com", FullName = "Command Tester" };
        db.Users.Add(user);
        await db.SaveChangesAsync();

        var dto = new MeetingCreateDto
        {
            Title = "Command Meeting",
            Description = "Command Description",
            StartTime = DateTime.Now.AddHours(2),
            EndTime = DateTime.Now.AddHours(3)
        };

        var command = new CreateMeetingCommand(
            userRepo,
            meetingRepo,
            dto,
            user.Id
        );

        await command.ExecuteAsync();

        var createdMeeting = db.Meetings.FirstOrDefault(m => m.Title == dto.Title);
        createdMeeting.Should().NotBeNull();
        createdMeeting!.OrganizerId.Should().Be(user.Id);
    }
}
