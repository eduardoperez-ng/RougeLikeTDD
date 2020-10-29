using UnityEngine;

namespace Completed
{
    public class Enemy : MovingObject
    {
        public int playerDamage;
        public AudioClip attackSound1;
        public AudioClip attackSound2;
        
        private Animator _animator;
        private Transform _target; 
        private bool _skipMove;
        
        private static readonly int EnemyAttack = Animator.StringToHash("enemyAttack");

        public override void Init()
        {
            base.Init();
            _target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        protected void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public override void AttemptMove<T>(int xDir, int yDir)
        {
            if (_skipMove)
            {
                _skipMove = false;
                return;
            }

            base.AttemptMove<T>(xDir, yDir);

            _skipMove = true;
        }


        public void MoveEnemy()
        {
            int xDir = 0;
            int yDir = 0;

            //If the difference in positions is approximately zero (Epsilon) do the following:
            if (Mathf.Abs(_target.position.x - transform.position.x) < float.Epsilon)
                //If the y coordinate of the target's (player) position is greater than the y coordinate of this enemy's position set y direction 1 (to move up). If not, set it to -1 (to move down).
                yDir = _target.position.y > transform.position.y ? 1 : -1;
            //If the difference in positions is not approximately zero (Epsilon) do the following:
            else
                //Check if target x position is greater than enemy's x position, if so set x direction to 1 (move right), if not set to -1 (move left).
                xDir = _target.position.x > transform.position.x ? 1 : -1;

            //Call the AttemptMove function and pass in the generic parameter Player, because Enemy is moving and expecting to potentially encounter a Player
            AttemptMove<Player>(xDir, yDir);
        }


        public override void OnCantMove<T>(T component)
        {
            //Declare hitPlayer and set it to equal the encountered component.
            Player hitPlayer = component as Player;

            //Call the LoseFood function of hitPlayer passing it playerDamage, the amount of foodpoints to be subtracted.
            hitPlayer.LoseFood(playerDamage);

            //Set the attack trigger of animator to trigger Enemy attack animation.
            _animator.SetTrigger(EnemyAttack);

            //Call the RandomizeSfx function of SoundManager passing in the two audio clips to choose randomly between.
            SoundManager.instance.RandomizeSfx(attackSound1, attackSound2);
        }
    }
}