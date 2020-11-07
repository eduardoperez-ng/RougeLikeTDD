using Completed.Interfaces;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Completed.View
{
    public class GameManagerView : MonoBehaviour
    {
        public Text levelText;
        public Text timerText;
        public Text turnText;
        public Text currentDayText;
        public GameObject levelImage;
        public Button levelButton;
        
        private float hideLevelImageDelay = 2f;
        private ITimer _timer;
        private ILevelManager _levelManager;
        
        public void Init(ITimer timer, ILevelManager levelManager)
        {
            _timer = timer;
            _levelManager = levelManager;
            
            if (levelText == null)
            {
                levelText = GameObject.Find("LevelText").GetComponent<Text>();
            }
            if (levelImage == null)
            {
                levelImage = GameObject.Find("LevelImage");
            }

            if (levelButton == null) return;
            
            levelButton.onClick.AddListener(LoadMainMenu);
            levelButton.gameObject.SetActive(false);
        }

        public void ShowCurrentDay(int currentDay)
        {
            levelText.text = $"Day {currentDay}";
            currentDayText.text = $"day {currentDay}";
            levelImage.SetActive(true);
            Invoke(nameof(HideLevelImage), hideLevelImageDelay);
        }

        private void HideLevelImage()
        {
            levelImage.SetActive(false);
        }

        public void ShowGameOver(int currentDay)
        {
            levelText.text = $"After {currentDay} days, you starved.";
            levelImage.SetActive(true);
            levelButton.gameObject.SetActive(true);
        }
        
        private static void LoadMainMenu()
        {
            SceneManager.LoadSceneAsync("MainMenu");
        }

        public void ShowElapsedTime()
        {
            timerText.text = $"{_timer.ElapsedTime:0000}";
        }
        
        public void ShowTurn(string gameActor)
        {
            turnText.color = gameActor == "Player" ? Color.green : Color.white;
            turnText.text = gameActor;
        }
    }
}