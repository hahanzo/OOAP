using OOAP_lab7.Server.Commands;

namespace OOAP_lab7.Server.Services
{
    public class CommandManager
    {
        private readonly Stack<ICommand> _history = new Stack<ICommand>();

        public async Task ExecuteCommandAsync(ICommand command)
        {
            await command.ExecuteAsync();
            _history.Push(command);
        }

        public async Task UndoLastCommandAsync()
        {
            if (_history.Count > 0)
            {
                ICommand lastCommand = _history.Pop();
                await lastCommand.UndoAsync();
            }
        }
    }
}
