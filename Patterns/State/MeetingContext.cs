using Patterns.State.States;

namespace Patterns.State;

public class MeetingContext
{
    public IMeetingState State { get; private set; }

    public MeetingContext()
    {
        State = new CreatedState();
    }

    public void SetState(IMeetingState newState)
    {
        State = newState;
    }

    public void Edit() => State.Edit(this);
    public void Cancel() => State.Cancel(this);
    public void Start() => State.Start(this);
    public void Finish() => State.Finish(this);
    public void SaveEdit() => State.SaveEdit(this);
}
