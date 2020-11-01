using System.Collections.Generic;
using Completed.Interfaces;
using UnityEngine;

namespace Completed.Commands.Logger
{
    public class InMemoryCommandLogger : ICommandLogger
    {
        private Dictionary<float, Command> _executedCommands;
        private ITimer _timer;

        public InMemoryCommandLogger(ITimer timer)
        {
            _timer = timer;
            _executedCommands = new Dictionary<float, Command>();
        }

        public void LogCommand(Command command)
        {
            float elapsedTime = _timer.ElapsedTime;
            _executedCommands.Add(elapsedTime, command);
        }
        
    }
}