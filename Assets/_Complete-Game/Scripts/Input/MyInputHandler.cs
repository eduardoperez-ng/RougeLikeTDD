using Completed.Commands;
using UnityEngine;

namespace Completed.MyInput
{
    public class MyInputHandler : MonoBehaviour
    {
        [SerializeField] private bool _started;
        
        private int _horizontal = 0;
        private int _vertical = 0;
        
        public MyCommandEvent commandPipeline = new MyCommandEvent();
        
        private void Update()
        {
            if (!_started)
                return;
            
            _horizontal = (int) (Input.GetAxisRaw("Horizontal"));
            _vertical = (int) (Input.GetAxisRaw("Vertical"));
            
            var command = CreateCommandFromInput();
            
            if (command != null)
                SendCommand(command);
        }

        private Command CreateCommandFromInput()
        {
            if (_horizontal != 0)
            {
                _vertical = 0;
                if (_horizontal > 0)
                    return new MoveRightCommand();
                
                return new MoveLeftCommand();
            }

            if (_vertical != 0)
            {
                _horizontal = 0;
                if (_vertical > 0)
                    return new MoveUpCommand();

                return new MoveDownCommand();
            }

            return null;
        }

        private void SendCommand(Command command)
        {
            commandPipeline?.Invoke(command);
        }
    }
}