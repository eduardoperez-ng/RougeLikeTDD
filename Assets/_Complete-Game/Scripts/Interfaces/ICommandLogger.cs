using Completed.Commands;

namespace Completed.Interfaces
{
    public interface ICommandLogger
    {
        void LogCommand(Command command);
    }
}