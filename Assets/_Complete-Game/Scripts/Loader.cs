using UnityEngine;

namespace Completed
{
    public class Loader : MonoBehaviour
    {
        public GameObject gameManager; 
        public GameObject soundManager;
        public GameObject ghostManager;
        
        private void Awake()
        {
            CreateGhostManager();
            CreateGameManager();
            CreateSoundManager();
        }

        private void CreateGhostManager()
        {
            if (FindObjectOfType<GhostManager>() == null)
            {
                var shadowManagerInstance = Instantiate(ghostManager);
                DontDestroyOnLoad(shadowManagerInstance);
            }
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