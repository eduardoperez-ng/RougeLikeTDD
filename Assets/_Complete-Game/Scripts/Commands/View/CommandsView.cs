using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CommandsView : MonoBehaviour
{
    public Text _commandText;

    public void UpdateView(string command)
    {
        _commandText.text += $"{command} ";
    }
    
    public void ClearView()
    {
        _commandText.text = string.Empty;
    }
}
