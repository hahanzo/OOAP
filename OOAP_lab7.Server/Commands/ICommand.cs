namespace OOAP_lab7.Server.Commands
{
    public interface ICommand
    {
        Task ExecuteAsync();
        Task UndoAsync();
    }
}
