using UnityEngine;

namespace Completed
{
    public class PlayerGhost : MovingObject
    {
        private static readonly int PlayerHit = Animator.StringToHash("playerHit");
        private static readonly int PlayerChop = Animator.StringToHash("playerChop");

        public int wallDamage = 1;
        private Animator animator;
        
        public override void Init()
        {
            InitAnimator();
            base.Init();
        }

        private void InitAnimator()
        {
            animator = GetComponent<Animator>();
        }
        
        public override void AttemptMove<T>(int xDir, int yDir)
        {
            base.AttemptMove<T>(xDir, yDir);
            Move(xDir, yDir, out _);
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
                enabled = false;
                return;
            }
            
            if (other.CompareTag("Food"))
            {
                other.gameObject.SetActive(false);
                return;
            }
            
            if (other.CompareTag("Soda"))
            {
                other.gameObject.SetActive(false);
            }
        }

    }
}