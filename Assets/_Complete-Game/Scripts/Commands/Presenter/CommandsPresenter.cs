using Completed.Interfaces;

namespace Completed.Commands.Presenter
{
    public class CommandsPresenter : ICommandLogger
    {
        private CommandsView _commandsView;
        
        public CommandsPresenter(CommandsView commandView)
        {
            _commandsView = commandView;
        }
        
        public void LogCommand(Command command)
        {
            _commandsView.UpdateView(command.ToString());
        }

        public void RemoveLastLoggedCommand() {}
    }
}