using Domain.Interfaces;
using FluentAssertions;
using NSubstitute;
using Patterns.Strategy;

namespace UnitTest;

public class StrategyTests
{
	[Fact]
	public async Task IsSlotAvailableAsync_ReturnsTrue_WhenNoConflicts()
	{
		var calendarRepo = Substitute.For<ICalendarRepository>();
		calendarRepo.IsSlotAvailableAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>())
					.Returns(true);

		var strategy = new StandardSlotStrategy(calendarRepo);

		var result = await strategy.IsSlotAvailableAsync(DateTime.Now, DateTime.Now.AddHours(1), 1);
		result.Should().BeTrue();
	}
}
