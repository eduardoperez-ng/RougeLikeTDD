using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Completed.Commands;
using Completed.Interfaces;
using Completed.MyInput;
using Completed.Presenter;
using Completed.View;

namespace Completed
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        public float turnDelay = 0.1f;

        // TODO: move this to a turn manager.
        [HideInInspector]
        public bool playersTurn = true;

        //private Text levelText; 
        //private GameObject levelImage;
        private BoardManager boardScript;
        private EnemyManager _enemyManager;
        
        private bool enemiesMoving;
        private bool doingSetup = true;

        private Player _player;
        private PlayerPresenter _playerPresenter;
        private GameManagerPresenter _gameManagerPresenter;
        private MyInputHandler _myInputHandler;
        
        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            Debug.Log("GameManager::Init()");
            doingSetup = true;
            InitInput();
            InitLevel();
            InitPlayer();
            InitBoard();
            InitEnemies();
            InitUi();
            doingSetup = false;
        }

        private void InitInput()
        {
            if (_myInputHandler == null)
            {
                _myInputHandler = GameObject.Find("MyInputHandler").GetComponent<MyInputHandler>();
                _myInputHandler.commandPipeline.AddListener(HandleInput);
            }
        }

        private static void InitLevel()
        {
            if (LevelManager.CurrentDay == 0)
            {
                LevelManager.CurrentDay = 1;
            }
        }

        private void HandleInput(Command command)
        {
            Debug.Log($"*** HandleInput: {command}");
            if (IsPlayersTurn() && !_player.IsMoving())
            {
                command.Execute(_player);
            }
        }

        // TODO: add a presenter to handle this.
        private void InitUi()
        {
            GameManagerView gameManagerView = GameObject.Find("LevelImage").GetComponent<GameManagerView>(); 
            _gameManagerPresenter = new GameManagerPresenter(this, gameManagerView);
            _gameManagerPresenter.ShowCurrentDay(LevelManager.CurrentDay);
            // if (levelText == null)
            // {
            //     levelText = GameObject.Find("LevelText").GetComponent<Text>();
            // }
            // levelText.text = "Day " + LevelManager.CurrentDay;
            //
            // if (levelImage == null)
            // {
            //     levelImage = GameObject.Find("LevelImage");
            // }
            // levelImage.SetActive(true);
            //
            // Invoke(nameof(HideLevelImage), levelStartDelay);
        }
        
        private void InitPlayer()
        {
            if (_player == null)
            {
                _player = FindObjectOfType<Player>();
                _player.Init(LevelManager.GetPlayerFoodForCurrentDay());
            }

            if (_playerPresenter == null)
            {
                var playerView = FindObjectOfType<PlayerView>();
                _playerPresenter = new PlayerPresenter(_player, playerView);
            }
            
            _player.PlayerTurnEndEvent.AddListener(HandleEndPlayerTurn);
            _player.PlayerReachedExitEvent.AddListener(HandlePlayerReachedExit);
            _player.PlayerDeadEvent.AddListener(GameOver);
        }

        public void HandleEndPlayerTurn()
        {
            playersTurn = false;
        }
        
        private void HandlePlayerReachedExit()
        {
            IncreaseLevel();
            SavePlayerFood();
            StartCoroutine(LoadNextLevel());
        }
        
        private static void IncreaseLevel()
        {
            LevelManager.CurrentDay++;
        }

        private void SavePlayerFood()
        {
            LevelManager.CurrentPlayerFood = _player.Food;
        }

        private IEnumerator LoadNextLevel()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadSceneAsync("Main");
        }
        
        public void GameOver()
        {
            _gameManagerPresenter.ShowGameOver(LevelManager.CurrentDay);
            enabled = false;
            LevelManager.CurrentDay = 1;
        }

        private void InitEnemies()
        {
            _enemyManager = new EnemyManager(boardScript.InstantiatedEnemies);
        }

        private void InitBoard()
        {
            if (boardScript == null)
            {
                boardScript = GetComponent<BoardManager>();
            }
            boardScript.SetupScene(LevelManager.CurrentDay);
        }

        private void Update()
        {
            if (doingSetup)
                return;
            
            if (playersTurn || enemiesMoving)
                return;

            StartCoroutine(MoveEnemies());
        }
        public IEnumerator MoveEnemies()
        {
            Debug.Log("MoveEnemies");
            enemiesMoving = true;
            yield return new WaitForSeconds(turnDelay);

            if (_enemyManager.HasEnemies())
            {
                yield return new WaitForSeconds(turnDelay);
            }

            foreach (var enemy in _enemyManager.Enemies)
            {
                enemy.MoveEnemy();
                yield return new WaitForSeconds(enemy.moveTime);
            }

            playersTurn = true;
            enemiesMoving = false;
        }

        public bool IsPlayersTurn()
        {
            return playersTurn;
        }
        
    }
}