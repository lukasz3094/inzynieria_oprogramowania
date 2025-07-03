namespace Domain.Interfaces;

public interface IMeetingSchedulingStrategy
{
    Task<bool> IsSlotAvailableAsync(DateTime start, DateTime end, int userId);
}
