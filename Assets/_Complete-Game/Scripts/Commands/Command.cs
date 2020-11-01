using UnityEngine.Events;

namespace Completed.Commands
{
    [System.Serializable]
    public class Command
    {
        // TODO: actualizar la interfaz para que devuelva bool,
        // asi podemos esperar a que termine de ejecutarse el movimiento
        // para ejecutar el proximo.
        public virtual void Execute(GameActor gameActor) {}
    }

    [System.Serializable]
    public class MyCommandEvent : UnityEvent<Command>
    {
        
    }
}