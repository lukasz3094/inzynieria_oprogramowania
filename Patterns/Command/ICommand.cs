namespace Patterns.Command;

public interface ICommand
{
    Task ExecuteAsync();
}
