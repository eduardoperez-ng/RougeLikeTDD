using System.Collections.Generic;
using Completed.Interfaces;

namespace Completed.Commands.Logger
{
    public class InMemoryCommandLogger : ICommandLogger
    {
        private Dictionary<int, List<Command>> _executedCommands;
        private ILevelManager _levelManager;
        
        public InMemoryCommandLogger(ILevelManager levelManager)
        {
            _executedCommands = new Dictionary<int, List<Command>>();
            _levelManager = levelManager;
        }

        public void LogCommand(Command command)
        {
            int currentDay = GetCurrentDay();
            if (_executedCommands.TryGetValue(currentDay, out var commands))
            {
                commands.Add(command);
            }
            else
            {
                _executedCommands.Add(currentDay, new List<Command>{command});
            }
        }

        private int GetCurrentDay()
        {
            return _levelManager.CurrentDay;
        }

        public IReadOnlyList<Command> CommandsForDay(int day)
        {
            return _executedCommands.TryGetValue(day, out var commands) ? commands : null;
        }

        public void RemoveLastLoggedCommand()
        {
            if (_executedCommands == null)
                return;

            if (_executedCommands.TryGetValue(GetCurrentDay(), out var commands))
            {
                commands.RemoveAt(commands.Count - 1);
            }
        }
    }
}