using UnityEngine.Events;

namespace Completed.Commands
{
    [System.Serializable]
    public class Command
    {
        public virtual void Execute() {}
    }

    [System.Serializable]
    public class MyCommandEvent : UnityEvent<Command>
    {
        
    }
}