using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Completed.View
{
    public class GameManagerView : MonoBehaviour
    {
        public Text levelText; 
        public GameObject levelImage;
        public Button levelButton;
        private float hideLevelImageDelay = 2f;
        
        public void Init()
        {
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
            levelText.text = $"Day {LevelManager.CurrentDay}";
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
    }
}