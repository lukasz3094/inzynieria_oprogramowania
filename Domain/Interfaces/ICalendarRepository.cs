namespace Domain.Interfaces;

public interface ICalendarRepository
{
	Task<bool> IsSlotAvailableAsync(DateTime startTime, DateTime endTime, int userId);
}