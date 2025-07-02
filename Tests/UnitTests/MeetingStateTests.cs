using FluentAssertions;
using Patterns.State;
using Xunit;

namespace UnitTests.State;

public class MeetingStateTests
{
    [Fact]
    public void CreatedMeeting_CanBeEditedAndCancelledOrStarted()
    {
        var context = new MeetingContext();
        context.State.Name.Should().Be("Created");

        context.Edit();
        context.State.Name.Should().Be("Edited");

        context.SaveEdit();
        context.State.Name.Should().Be("Created");

        context.Start();
        context.State.Name.Should().Be("InProgress");

        context.Finish();
        context.State.Name.Should().Be("Finished");
    }

    [Fact]
    public void EditedMeeting_CannotStartWithoutSaving()
    {
        var context = new MeetingContext();
        context.Edit();

        Action act = () => context.Start();
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*before saving*");
    }

    [Fact]
    public void CancelledMeeting_CannotBeModified()
    {
        var context = new MeetingContext();
        context.Cancel();

        Action edit = () => context.Edit();
        Action start = () => context.Start();
        Action finish = () => context.Finish();

        edit.Should().Throw<InvalidOperationException>();
        start.Should().Throw<InvalidOperationException>();
        finish.Should().Throw<InvalidOperationException>();
    }
}
