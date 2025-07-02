namespace Patterns.State;

public interface IMeetingState
{
    void Edit(MeetingContext context);
    void Cancel(MeetingContext context);
    void Start(MeetingContext context);
    void Finish(MeetingContext context);
    void SaveEdit(MeetingContext context);
    string Name { get; }
}
