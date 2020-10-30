using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuView : MonoBehaviour
{
    public Button startGameButton;

    private void Start()
    {
        if (startGameButton != null)
        {
            startGameButton.onClick.AddListener(StartGame);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync("Main");
    }
}
