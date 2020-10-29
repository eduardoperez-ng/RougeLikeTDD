using UnityEngine;
using UnityEngine.UI;

namespace Completed.View
{
    public class GameManagerView : MonoBehaviour
    {
        public Text levelText; 
        public GameObject levelImage;
        private float hideLevelImageDelay = 2f;

        public void Init()
        {
            if (levelText == null)
            {
                levelText = GameObject.Find("LevelText").GetComponent<Text>();
            }
            //levelText.text = "Day " + LevelManager.CurrentDay;

            if (levelImage == null)
            {
                levelImage = GameObject.Find("LevelImage");
            }
            //levelImage.SetActive(true);
            //Invoke(nameof(HideLevelImage), hideLevelImageDelay);
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
        }
    }
}