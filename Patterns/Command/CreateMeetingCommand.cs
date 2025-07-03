using Contracts.DTOs.Api;
using Domain.Entities;
using Domain.Interfaces;

namespace Patterns.Command;

public class CreateMeetingCommand(
    IUserRepository userRepository,
    IMeetingRepository meetingRepository,
    MeetingCreateDto meetingDto,
    int userId) : ICommand
{
    public async Task ExecuteAsync()
    {
        var user = await userRepository.GetByIdAsync(userId) ?? throw new InvalidOperationException("User not found.");
		var meeting = new Meeting
        {
            Title = meetingDto.Title,
            Description = meetingDto.Description,
            StartTime = meetingDto.StartTime,
            EndTime = meetingDto.EndTime,
            OrganizerId = userId
        };

        await meetingRepository.AddAsync(meeting);
        await meetingRepository.SaveChangesAsync();
    }
}
