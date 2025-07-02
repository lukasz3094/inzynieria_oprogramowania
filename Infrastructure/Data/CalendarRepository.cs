using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;

namespace Infrastructure.Data;

public class CalendarRepository(IMeetingRepository meetingRepository) : ICalendarRepository
{
	private readonly IMeetingRepository _meetingRepository = meetingRepository;

	public async Task<bool> IsSlotAvailableAsync(DateTime startTime, DateTime endTime, int userId)
	{
		var meetings = await _meetingRepository.GetByOrganizerIdAsync(userId);
		foreach (var meeting in meetings)
			if (meeting.StartTime < endTime && startTime < meeting.EndTime)
				return false;

		return true;
	}
}
