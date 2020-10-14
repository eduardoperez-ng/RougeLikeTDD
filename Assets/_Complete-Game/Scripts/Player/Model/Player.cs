using Completed.Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI; 
using UnityEngine.SceneManagement;

namespace Completed
{
    public class Player : MovingObject
    {
        private static readonly int PlayerHit = Animator.StringToHash("playerHit");
        
        public float restartLevelDelay = 1f; //Delay time in seconds to restart level.
        public int pointsPerFood = 10; //Number of points to add to player food points when picking up a food object.
        public int pointsPerSoda = 20; //Number of points to add to player food points when picking up a soda object.
        public int wallDamage = 1; //How much damage a player does to a wall when chopping it.
        
        //public Text foodText;
        public int Food { get; private set; }

        public AudioClip moveSound1;
        public AudioClip moveSound2;
        public AudioClip eatSound1;
        public AudioClip eatSound2;
        public AudioClip drinkSound1;
        public AudioClip drinkSound2;
        public AudioClip gameOverSound;

        private Animator animator;
        private IGameManager _gameManager;
        
        // TODO: add list of executed movements.
        public UnityEvent PlayerMoveEvent = new UnityEvent();
        
        public void Init(IGameManager gameManager, int initialFoodPoint)
        {
            _gameManager = gameManager;
            Food = initialFoodPoint;

            InitAnimator();
            
            base.Init();
        }

        private void InitAnimator()
        {
            animator = GetComponent<Animator>();
        }

        // private void OnDisable()
        // {
        //     GameManager.instance.playerFoodPoints = _food;
        // }

        private void Update()
        {
            if (_gameManager == null)
                return;

            if (_gameManager.IsPlayersTurn())
            {
                int horizontal = 0;
                int vertical = 0;

                horizontal = (int) (Input.GetAxisRaw("Horizontal"));
                vertical = (int) (Input.GetAxisRaw("Vertical"));

                if (horizontal != 0)
                {
                    vertical = 0;
                }

                if (horizontal != 0 || vertical != 0)
                {
                    AttemptMove<Wall>(horizontal, vertical);
                }
            }
        }

        public override void AttemptMove<T>(int xDir, int yDir)
        {
            Food--;
            PublishMoveEvent();
            
            base.AttemptMove<T>(xDir, yDir);

            if (Move(xDir, yDir, out RaycastHit2D hit))
            {
                SoundManager.instance.RandomizeSfx(moveSound1, moveSound2);
            }

            CheckIfGameOver();

            EndTurn();
        }

        private void PublishMoveEvent()
        {
            PlayerMoveEvent?.Invoke();
        }

        private void EndTurn()
        {
            _gameManager.EndPlayerTurn();
        }
        
        public override void OnCantMove<T>(T component)
        {
            var hitWall = component as Wall;

            if (hitWall != null) 
                hitWall.DamageWall(wallDamage);

            animator.SetTrigger("playerChop");
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Exit"))
            {
                Invoke("Restart", restartLevelDelay);
                enabled = false;
                return;
            }
            
            if (other.CompareTag("Food"))
            {
                Food += pointsPerFood;
                //foodText.text = "+" + pointsPerFood + " Food: " + Food;

                SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);

                other.gameObject.SetActive(false);
                return;
            }
            
            if (other.CompareTag("Soda"))
            {
                Food += pointsPerSoda;
                //foodText.text = "+" + pointsPerSoda + " Food: " + Food;

                SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);

                other.gameObject.SetActive(false);
                return;
            }
        }
        
        private void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }
        
        public void LoseFood(int loss)
        {
            animator.SetTrigger(PlayerHit);

            Food -= loss;
            //foodText.text = "-" + loss + " Food: " + Food;

            CheckIfGameOver();
        }
        
        private void CheckIfGameOver()
        {
            if (Food > 0) return;
            
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            
            _gameManager.GameOver();
        }
    }
}