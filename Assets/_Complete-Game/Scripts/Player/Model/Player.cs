﻿using Completed.Constants;
using Completed.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Completed
{
    public class Player : MovingObject
    {
        private static readonly int PlayerHit = Animator.StringToHash("playerHit");

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
            // TODO: add an event
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
                Invoke(nameof(OnPlayerReachedExit), 1f);
                enabled = false;
                return;
            }
            
            if (other.CompareTag("Food"))
            {
                Food += FoodConstants.PointsPerFood;
                // TODO: every time we hit something use a callback so we can update the view.
                //foodText.text = "+" + pointsPerFood + " Food: " + Food;

                SoundManager.instance.RandomizeSfx(eatSound1, eatSound2);

                other.gameObject.SetActive(false);
                return;
            }
            
            if (other.CompareTag("Soda"))
            {
                Food += FoodConstants.PointsPerSoda;
                //foodText.text = "+" + pointsPerSoda + " Food: " + Food;

                SoundManager.instance.RandomizeSfx(drinkSound1, drinkSound2);

                other.gameObject.SetActive(false);
            }
        }

        private void OnPlayerReachedExit()
        {
            SetPosition(0,0);
            // TODO: replace this with an event.
            _gameManager.LoadNextLevel();
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
            
            // TODO: add an event.
            _gameManager.GameOver();
        }

        public bool IsMoving()
        {
            return Moving;
        }
    }
}