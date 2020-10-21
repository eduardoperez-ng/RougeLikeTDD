using Completed.Commands;
using Completed.MyInput;
using UnityEngine;

public class MyInputHandlerTest : MonoBehaviour
{
    private MyInputHandler _myInputHandler;

    // Start is called before the first frame update
    private void Start()
    {
        if (_myInputHandler == null)
        {
            _myInputHandler = GameObject.Find("MyInputHandler").GetComponent<MyInputHandler>();
            _myInputHandler.commandPipeline.AddListener(HandleInput);
            _myInputHandler.Unlock();
        }
    }

    private static void HandleInput(Command command)
    {
        Debug.Log($"** Command: {command}");
    }
}
