namespace Patterns.State.States;

public class EditedState : IMeetingState
{
    public string Name => "Edited";

    public void Edit(MeetingContext context) =>
        throw new InvalidOperationException("Already in edit mode.");

    public void Cancel(MeetingContext context) => context.SetState(new CancelledState());

    public void Start(MeetingContext context) =>
        throw new InvalidOperationException("Cannot start meeting before saving edits.");

    public void Finish(MeetingContext context) =>
        throw new InvalidOperationException("Cannot finish meeting before starting.");

    public void SaveEdit(MeetingContext context) => context.SetState(new CreatedState());
}
