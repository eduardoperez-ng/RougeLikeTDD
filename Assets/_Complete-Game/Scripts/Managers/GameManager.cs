using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 
using System.Collections;
using System.Collections.Generic; 
using Completed.Interfaces;

namespace Completed
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        public float levelStartDelay = 2f;
        public float turnDelay = 0.1f;
        private const int playerStartingFoodPoints = 100;

        //public static GameManager instance = null;
        
        [HideInInspector]
        public bool playersTurn = true;

        private Text levelText; 
        private GameObject levelImage;
        private BoardManager boardScript;
        private int level = 1; 
        private List<Enemy> enemies;
        private bool enemiesMoving; 

        private bool doingSetup = true;

        private Player _player;
        private PlayerPresenter _playerPresenter;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Init();
        }

        // public void CallbackInitialization()
        // {
        //     SceneManager.sceneLoaded += OnSceneLoaded;
        // }
        //
        // private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        // {
        //     Debug.Log("OnSceneLoaded()");
        //     level++;
        //     Init();
        // }
        
        public void Init()
        {
            Debug.Log("GameManager::Init()");
            doingSetup = true;
            InitUi();
            InitPlayer();
            InitEnemies();
            InitBoard();
        }

        private void InitUi()
        {
            levelImage = GameObject.Find("LevelImage");
            levelText = GameObject.Find("LevelText").GetComponent<Text>();
            levelText.text = "Day " + level;
            levelImage.SetActive(true);
            Invoke("HideLevelImage", levelStartDelay);
        }
        
        private void InitPlayer()
        {
            _player = FindObjectOfType<Player>();
            _player.Init(this, playerStartingFoodPoints);

            var playerView = FindObjectOfType<PlayerView>();
            _playerPresenter = new PlayerPresenter(_player, playerView);
        }

        private void InitEnemies()
        {
            if (enemies == null)
            {
                enemies = new List<Enemy>();
            }
            enemies.Clear();
        }

        private void InitBoard()
        {
            boardScript = GetComponent<BoardManager>();
            boardScript.SetupScene(level);
        }

        public void HideLevelImage()
        {
            levelImage.SetActive(false);
            doingSetup = false;
        }

        private void Update()
        {
            if (playersTurn || enemiesMoving || doingSetup)
                return;

            StartCoroutine(MoveEnemies());
        }

        public void AddEnemyToList(Enemy script)
        {
            enemies.Add(script);
        }
        
        public void GameOver()
        {
            levelText.text = "After " + level + " days, you starved.";
            levelImage.SetActive(true);
            enabled = false;
        }

        public IEnumerator MoveEnemies()
        {
            enemiesMoving = true;
            yield return new WaitForSeconds(turnDelay);

            if (enemies.Count == 0)
            {
                yield return new WaitForSeconds(turnDelay);
            }

            foreach (var enemy in enemies)
            {
                enemy.MoveEnemy();
                yield return new WaitForSeconds(enemy.moveTime);
            }

            playersTurn = true;
            enemiesMoving = false;
        }

        public void EndPlayerTurn()
        {
            playersTurn = false;
        }

        public bool IsPlayersTurn()
        {
            return playersTurn;
        }
    }
}