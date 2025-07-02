namespace Patterns.State.States;

public class InProgressState : IMeetingState
{
    public string Name => "InProgress";

    public void Edit(MeetingContext context) =>
        throw new InvalidOperationException("Cannot edit meeting while it is in progress.");

    public void Cancel(MeetingContext context) =>
        context.SetState(new CancelledState());

    public void Start(MeetingContext context) =>
        throw new InvalidOperationException("Meeting is already in progress.");

    public void Finish(MeetingContext context) =>
        context.SetState(new FinishedState());

    public void SaveEdit(MeetingContext context) =>
        throw new InvalidOperationException("Cannot save edit while meeting is in progress.");
}
