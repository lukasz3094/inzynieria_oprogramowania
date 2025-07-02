namespace Patterns.State.States;

public class CreatedState : IMeetingState
{
    public string Name => "Created";

    public void Edit(MeetingContext context) => context.SetState(new EditedState());

    public void Cancel(MeetingContext context) => context.SetState(new CancelledState());

    public void Start(MeetingContext context) => context.SetState(new InProgressState());

    public void Finish(MeetingContext context) =>
        throw new InvalidOperationException("Cannot finish a meeting that hasn't started.");

    public void SaveEdit(MeetingContext context) =>
        throw new InvalidOperationException("Nothing to save in created state.");
}
