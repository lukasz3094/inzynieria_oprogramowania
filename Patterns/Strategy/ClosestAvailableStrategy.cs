using Domain.Interfaces;

namespace Patterns.Strategy;

public class ClosestAvailableStrategy(ICalendarRepository calendarRepo) : IMeetingSchedulingStrategy
{
    private readonly ICalendarRepository _calendarRepo = calendarRepo;

    public async Task<bool> IsSlotAvailableAsync(DateTime start, DateTime end, int userId)
    {
        var originalDuration = end - start;

        if (await _calendarRepo.IsSlotAvailableAsync(start, end, userId))
            return true;

        var maxLookAhead = TimeSpan.FromHours(8);
        var interval = TimeSpan.FromMinutes(15);

        var attempts = (int)(maxLookAhead.TotalMinutes / interval.TotalMinutes);

        for (int i = 1; i <= attempts; i++)
        {
            var newStart = start.AddMinutes(i * interval.TotalMinutes);
            var newEnd = newStart.Add(originalDuration);

            if (await _calendarRepo.IsSlotAvailableAsync(newStart, newEnd, userId))
                return true;
        }

        return false;
    }
}
