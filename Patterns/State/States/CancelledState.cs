namespace Patterns.State.States;

public class CancelledState : IMeetingState
{
    public string Name => "Cancelled";

    public void Edit(MeetingContext context) =>
        throw new InvalidOperationException("Cannot edit a cancelled meeting.");

    public void Cancel(MeetingContext context) =>
        throw new InvalidOperationException("Meeting is already cancelled.");

    public void Start(MeetingContext context) =>
        throw new InvalidOperationException("Cannot start a cancelled meeting.");

    public void Finish(MeetingContext context) =>
        throw new InvalidOperationException("Cannot finish a cancelled meeting.");

    public void SaveEdit(MeetingContext context) =>
        throw new InvalidOperationException("Cannot save edit for a cancelled meeting.");
}
