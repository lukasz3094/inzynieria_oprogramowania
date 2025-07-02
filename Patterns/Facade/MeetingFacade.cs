namespace Patterns.Facade;

using Api.DTOs.Api;
using Domain.Entities;
using Domain.Interfaces;
using System.Threading.Tasks;

public class MeetingFacade(
	// IAuthService authService,
	IUserRepository userService,
	ICalendarRepository calendarService,
	IMeetingRepository meetingService) : IMeetingFacade
{
	// private readonly IAuthService _authService = authService;
	private readonly IUserRepository _userService = userService;
	private readonly ICalendarRepository _calendarService = calendarService;
	private readonly IMeetingRepository _meetingService = meetingService;

	public async Task<bool> PlanMeetingAsync(int userId, MeetingCreateDto meetingDto)
	{
		// var userId = _authService.GetUserIdFromToken(token);
		// if (userId == null) return false;

		var user = await _userService.GetByIdAsync(userId);
		if (user == null) return false;

		var isAvailable = await _calendarService.IsSlotAvailableAsync(meetingDto.StartTime, meetingDto.EndTime, userId);
		if (!isAvailable) return false;

		var meeting = new Meeting
		{
			Title = meetingDto.Title,
			Description = meetingDto.Description,
			StartTime = meetingDto.StartTime,
			EndTime = meetingDto.EndTime,
			OrganizerId = userId
		};

		await _meetingService.AddAsync(meeting);
		await _meetingService.SaveChangesAsync();
		
		return true;
	}
}
