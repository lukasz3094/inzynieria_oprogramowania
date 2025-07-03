using Domain.Interfaces;

namespace Patterns.Strategy;

public class StandardSlotStrategy(ICalendarRepository calendarRepo) : IMeetingSchedulingStrategy
{
    private readonly ICalendarRepository _calendarRepo = calendarRepo;

    public async Task<bool> IsSlotAvailableAsync(DateTime start, DateTime end, int userId)
    {
        return await _calendarRepo.IsSlotAvailableAsync(start, end, userId);
    }
}
