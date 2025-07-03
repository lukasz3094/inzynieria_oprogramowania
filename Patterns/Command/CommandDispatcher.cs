namespace Patterns.Command;

public class CommandDispatcher
{
    public static async Task DispatchAsync(ICommand command)
    {
        await command.ExecuteAsync();
    }
}
