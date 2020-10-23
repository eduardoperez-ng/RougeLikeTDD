using UnityEngine;
using UnityEngine.UI;

namespace Completed
{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Text _foodText;

        public void UpdateFoodText(string food)
        {
            _foodText.text = $"Food: {food}";;
        }
        
        public void UpdateText(string text)
        {
            _foodText.text = text;
        }
    }
}
