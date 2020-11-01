using System.Collections.Generic;
using Completed.Interfaces;

namespace Completed.Commands.Logger
{
    public class InMemoryCommandLogger : ICommandLogger
    {
        // TODO: testear esta clase.
        private Dictionary<int, List<Command>> _executedCommands;

        public InMemoryCommandLogger()
        {
            _executedCommands = new Dictionary<int, List<Command>>();
        }

        public void LogCommand(Command command)
        {
            int currentDay = LevelManager.CurrentDay;
            if (_executedCommands.TryGetValue(currentDay, out var commands))
            {
                commands.Add(command);
            }
            else
            {
                _executedCommands.Add(currentDay, new List<Command>{command});
            }
        }
        
    }
}