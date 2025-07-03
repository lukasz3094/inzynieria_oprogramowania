using Domain.Enums;

namespace Domain.Interfaces;

public interface ISchedulingStrategyFactory
{
	IMeetingSchedulingStrategy Get(SchedulingStrategyType type);
}
