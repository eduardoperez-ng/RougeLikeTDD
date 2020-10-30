using Completed.Constants;
using UnityEngine;
using UnityEngine.Events;

namespace Completed
{
    public class Player : MovingObject
    {
        private static readonly int PlayerHit = Animator.StringToHash("playerHit");
        private static readonly int PlayerChop = Animator.StringToHash("playerChop");

        public int wallDamage = 1;

        public int Food { get; private set; }

        public AudioClip moveSound1;
        public AudioClip moveSound2;
        public AudioClip eatSound1;
        public AudioClip eatSound2;
        public AudioClip drinkSound1;
        public AudioClip drinkSound2;
        public AudioClip gameOverSound;

        private Animator animator;
        
        public UnityEvent PlayerMoveEvent = new UnityEvent();
        public UnityEvent PlayerTurnEndEvent = new UnityEvent();
        public UnityEvent PlayerReachedExitEvent = new UnityEvent();
        public UnityEvent PlayerDeadEvent = new UnityEvent();
        public PlayerEvent PlayerCollisionEvent = new PlayerEvent();

        public void Init(int initialFoodPoint)
        {
            Food = initialFoodPoint;

            InitAnimator();
            
            base.Init();
        }

        private void InitAnimator()
        {
            animator = GetComponent<Animator>();
        }

        public override void AttemptMove<T>(int xDir, int yDir)
        {
            Food--;
            
            PublishMoveEvent();
            
            base.AttemptMove<T>(xDir, yDir);

            if (Move(xDir, yDir, out _))
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
            PlayerTurnEndEvent?.Invoke();
        }
        
        public override void OnCantMove<T>(T component)
        {
            var hitWall = component as Wall;
            if (hitWall != null)
            {
                hitWall.DamageWall(wallDamage);
            }

            animator.SetTrigger(PlayerChop);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Exit"))
            {
                PlayerReachedExitEvent?.Invoke();
                enabled = false;
                return;
            }
            
            if (other.CompareTag("Food"))
            {
                Food += FoodConstants.PointsPerFood;
                PlayerCollisionEvent?.Invoke("Food");
                SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);
                other.gameObject.SetActive(false);
                return;
            }
            
            if (other.CompareTag("Soda"))
            {
                Food += FoodConstants.PointsPerSoda;
                PlayerCollisionEvent?.Invoke("Soda");
                SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);
                other.gameObject.SetActive(false);
            }
        }

        public void LoseFood(int loss)
        {
            animator.SetTrigger(PlayerHit);
            Food -= loss;
            CheckIfGameOver();
        }
        
        private void CheckIfGameOver()
        {
            if (Food > 0) return;
            
            SoundManager.instance.PlaySingle(gameOverSound);
            SoundManager.instance.musicSource.Stop();
            
            PlayerDeadEvent?.Invoke();
        }

        public bool IsMoving()
        {
            return Moving;
        }
    }

    [System.Serializable]
    public class PlayerEvent : UnityEvent<string> {}
    
}