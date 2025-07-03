using Domain.Enums;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Patterns.Strategy;

public class SchedulingStrategyFactory(
	IServiceProvider provider) : ISchedulingStrategyFactory
{
	public IMeetingSchedulingStrategy Get(SchedulingStrategyType type)
	{
		return type switch
		{
			SchedulingStrategyType.Standard => provider.GetRequiredService<StandardSlotStrategy>(),
			SchedulingStrategyType.Closest => provider.GetRequiredService<ClosestAvailableStrategy>(),
			_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
		};
	}
}
