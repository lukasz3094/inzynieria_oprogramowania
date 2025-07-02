using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Patterns.State;
using Patterns.State.States;
using Xunit;

namespace UnitTest.State;

public class MeetingStateIntegrationTests
{
    private class MeetingStateAdapter : MeetingContext
    {
        private readonly Meeting _meeting;

        public MeetingStateAdapter(Meeting meeting)
        {
            _meeting = meeting;
            // ustawienie początkowego stanu na podstawie statusu
			SetState(meeting.Status switch
			{
				MeetingStatus.Scheduled => new CreatedState(),
				MeetingStatus.InProgress => new InProgressState(),
				MeetingStatus.Completed => new FinishedState(),
				MeetingStatus.Cancelled => new CancelledState(),
				_ => new CreatedState()
			});
        }

		public void SyncStateToMeeting()
		{
			_meeting.Status = State.Name switch
			{
				"Created" => MeetingStatus.Scheduled,
				"Edited" => MeetingStatus.Scheduled,
				"InProgress" => MeetingStatus.InProgress,
				"Finished" => MeetingStatus.Completed,
				"Cancelled" => MeetingStatus.Cancelled,
				_ => MeetingStatus.Scheduled
			};
		}
    }

    [Fact]
    public void Meeting_GoesThroughValidLifecycle()
    {
        var meeting = new Meeting { Title = "State Test", Status = MeetingStatus.Scheduled };

        var stateContext = new MeetingStateAdapter(meeting);

        stateContext.Edit();
        stateContext.SyncStateToMeeting();
        meeting.Status.Should().Be(MeetingStatus.Scheduled);

        stateContext.SaveEdit();
        stateContext.SyncStateToMeeting();
        meeting.Status.Should().Be(MeetingStatus.Scheduled);

        stateContext.Start();
        stateContext.SyncStateToMeeting();
        meeting.Status.Should().Be(MeetingStatus.InProgress);

        stateContext.Finish();
        stateContext.SyncStateToMeeting();
        meeting.Status.Should().Be(MeetingStatus.Completed);
    }

    [Fact]
    public void Meeting_CannotBeEditedWhenInProgress()
    {
        var meeting = new Meeting { Title = "Live Meeting", Status = MeetingStatus.InProgress };

        var stateContext = new MeetingStateAdapter(meeting);

        Action act = () =>
        {
            stateContext.Edit();
        };

        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*Cannot edit meeting while it is in progress*");
    }

    [Fact]
    public void CancelledMeeting_RejectsAllActions()
    {
        var meeting = new Meeting { Title = "Cancelled", Status = MeetingStatus.Cancelled };
        var ctx = new MeetingStateAdapter(meeting);

        Action[] all = [
            () => ctx.Edit(),
            () => ctx.SaveEdit(),
            () => ctx.Start(),
            () => ctx.Finish(),
            () => ctx.Cancel()
        ];

        foreach (var action in all)
        {
            action.Should().Throw<InvalidOperationException>();
        }
    }
}
