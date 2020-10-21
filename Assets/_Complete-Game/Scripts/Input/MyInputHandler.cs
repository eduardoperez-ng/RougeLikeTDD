using Completed.Commands;
using UnityEngine;

namespace Completed.MyInput
{
    public class MyInputHandler : MonoBehaviour
    {
        [SerializeField] private bool _locked;
        [SerializeField, Range(0,5)] private float _deltaTime = 1.0f;

        private int _horizontal = 0;
        private int _vertical = 0;

        private float _lastTime = 0;

        public MyCommandEvent commandPipeline = new MyCommandEvent();

        private void Update()
        {
            if (_locked)
            {
                return;
            }

            ReadInput();
            
            TryToSendCommand();
        }

        private void ReadInput()
        {
            _horizontal = (int) Input.GetAxisRaw("Horizontal");
            _vertical = (int) Input.GetAxisRaw("Vertical");
        }

        private void TryToSendCommand()
        {
            if (_lastTime + _deltaTime < Time.time)
            {
                CreateAndSendCommand();
                _lastTime = Time.time;
            }
        }

        private void CreateAndSendCommand()
        {
            var command = CreateCommandFromInput();
            if (command != null)
            {
                commandPipeline?.Invoke(command);
            }
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

        public void Unlock()
        {
            _locked = false;
            _lastTime = Time.time;
        }

        public void Lock()
        {
            _locked = true;
        }
    }
}