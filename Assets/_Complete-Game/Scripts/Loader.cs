using UnityEngine;

namespace Completed
{
    public class Loader : MonoBehaviour
    {
        public GameObject gameManager; 
        public GameObject soundManager;
        
        private void Awake()
        {
            CreateGameManager();
            CreateSoundManager();
        }

        private void CreateGameManager()
        {
            if (FindObjectOfType<GameManager>() == null)
            {
                Instantiate(gameManager);
            }
        }

        private void CreateSoundManager()
        {
            if (SoundManager.instance == null)
            {
                Instantiate(soundManager);
            }
        }
    }
}