namespace Patterns.State.States;

public class FinishedState : IMeetingState
{
    public string Name => "Finished";

    public void Edit(MeetingContext context) =>
        throw new InvalidOperationException("Cannot edit a finished meeting.");

    public void Cancel(MeetingContext context) =>
        throw new InvalidOperationException("Cannot cancel a finished meeting.");

    public void Start(MeetingContext context) =>
        throw new InvalidOperationException("Meeting is already finished.");

    public void Finish(MeetingContext context) =>
        throw new InvalidOperationException("Meeting is already finished.");

    public void SaveEdit(MeetingContext context) =>
        throw new InvalidOperationException("Cannot save edit for a finished meeting.");
}
