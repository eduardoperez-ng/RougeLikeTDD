using UnityEngine;
using UnityEngine.UI;

namespace Completed
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Text foodText;

        public void UpdateFoodText(string food)
        {
            foodText.text = $"Food: {food}";;
        }
        
        public void UpdateText(string text)
        {
            foodText.text = text;
        }
    }
}
