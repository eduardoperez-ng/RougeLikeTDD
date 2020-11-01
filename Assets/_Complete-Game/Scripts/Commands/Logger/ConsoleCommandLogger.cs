using Completed.Interfaces;
using UnityEngine;

namespace Completed.Commands.Logger
{
    public class ConsoleCommandLogger : ICommandLogger
    {
        private ITimer _timer;

        public ConsoleCommandLogger(ITimer timer)
        {
            _timer = timer;
        }

        public void LogCommand(Command command)
        {
            Debug.Log($"{_timer.ElapsedTime}: {command}");
        }
    }
}