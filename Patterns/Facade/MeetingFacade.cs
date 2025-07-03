namespace Patterns.Facade;

using Contracts.DTOs.Api;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Patterns.Strategy;
using System.Threading.Tasks;

public class MeetingFacade(
	// IAuthService authService,
	IUserRepository userService,
	ISchedulingStrategyFactory strategyFactory,
	IMeetingRepository meetingService) : IMeetingFacade
{
	// private readonly IAuthService _authService = authService;
	private readonly IUserRepository _userService = userService;
	private readonly ISchedulingStrategyFactory _strategyFactory = strategyFactory;	
	private readonly IMeetingRepository _meetingService = meetingService;

	public async Task<bool> PlanMeetingAsync(int userId, MeetingCreateDto dto, SchedulingStrategyType strategyType = SchedulingStrategyType.Standard)
    {
        var strategy = _strategyFactory.Get(strategyType);

        var user = await _userService.GetByIdAsync(userId);
        if (user == null) return false;

        bool isAvailable = await strategy.IsSlotAvailableAsync(dto.StartTime, dto.EndTime, userId);
        if (!isAvailable) return false;

        var meeting = new Meeting
		{
			Title = dto.Title,
			Description = dto.Description,
			StartTime = dto.StartTime,
			EndTime = dto.EndTime,
			OrganizerId = userId,
			Status = MeetingStatus.Scheduled
		};

        await _meetingService.AddAsync(meeting);
        await _meetingService.SaveChangesAsync();

        return true;
    }
}
