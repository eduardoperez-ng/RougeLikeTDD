using System.Collections;
using System.Collections.Generic;
using Completed.Commands;
using Completed.Commands.Logger;
using Completed.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Completed
{
    public class GhostManager : MonoBehaviour
    {
        public GhostManagerEvent CommandExecutedEvent = new GhostManagerEvent();

        private InMemoryCommandLogger _commandLogger;
        private PlayerGhost _playerGhost;

        public void Init(ICommandLogger commandLogger, PlayerGhost playerGhost)
        {
            _commandLogger = (InMemoryCommandLogger)commandLogger;
            _playerGhost = playerGhost;
            _playerGhost.Init();
        }

        public bool TryMoveGhostForDay(int day)
        {
            var commands = _commandLogger.CommandsForDay(day);
            if (commands != null && commands.Count > 0)
            {
                StartCoroutine(MoveGhost(commands));
                return true;
            }
            return false;
        }

        private IEnumerator MoveGhost(IReadOnlyList<Command> commands)
        {
            foreach (var command in commands)
            {
                command.Execute(_playerGhost);
                CommandExecutedEvent?.Invoke(command.ToString());
                yield return new WaitForSeconds(1);
            }
        }

        public void StopGhost()
        {
            StopAllCoroutines();
        }
    }
    
    [System.Serializable]
    public class GhostManagerEvent : UnityEvent<string> {}

}