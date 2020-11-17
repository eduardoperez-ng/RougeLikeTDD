using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using Completed.Commands;
using Completed.Commands.Logger;
using Completed.Commands.Presenter;
using Completed.Interfaces;
using Completed.MyInput;
using Completed.Presenter;
using Completed.Timer;
using Completed.View;

namespace Completed
{
    public class GameManager : MonoBehaviour, IGameManager
    {
        public float turnDelay = 0.1f;

        // TODO: move this to a turn manager.
        [HideInInspector]
        public bool playersTurn = true;
        private bool enemiesMoving;

        private BoardManager _boardManager;
        private EnemyManager _enemyManager;

        private bool doingSetup = true;

        private Player _player;
        private PlayerPresenter _playerPresenter;
        private GameManagerPresenter _gameManagerPresenter;
        private CommandsPresenter _commandsPresenter;
        private GhostManagerPresenter _ghostManagerPresenter;
        private MyInputHandler _myInputHandler;

        private ITimer _timer;
        private ICommandLogger _inMemoryCommandLogger;
        private ICommandLogger _consoleCommandLogger;
        private ILevelManager _levelManager;

        private GhostManager _ghostManager;
        private PlayerGhost _playerGhost;
        
        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            _gameManagerPresenter.ShowElapsedTime();
        }

        public void Init()
        {
            Debug.Log("GameManager::Init()");
            doingSetup = true;
            InitInput();
            InitLevelManager();
            InitPlayer();
            InitPlayerGhost();
            InitBoard();
            InitEnemies();
            InitTimer();
            InitUi();
            InitCommandLogger();
            InitGhostManager();
            doingSetup = false;
        }

        private void InitInput()
        {
            if (_myInputHandler != null) return;
            _myInputHandler = GameObject.Find("MyInputHandler").GetComponent<MyInputHandler>();
            _myInputHandler.commandPipeline.AddListener(HandleInput);
        }

        private void HandleInput(Command command)
        {
            if (IsPlayersTurn() && !_player.IsMoving())
            {
                command.Execute(_player);
                LogCommand(command);
            }
        }

        private void LogCommand(Command command)
        {
            _inMemoryCommandLogger.LogCommand(command);
            _commandsPresenter.LogCommand(command);
            _consoleCommandLogger.LogCommand(command);
        }

        private void InitLevelManager()
        {
            _levelManager = new LevelManager();
            if (_levelManager.CurrentDay == 0)
            {
                _levelManager.CurrentDay = 1;
            }
        }

        private void InitPlayer()
        {
            if (_player == null)
            {
                _player = FindObjectOfType<Player>();
                _player.Init(StaticLevelManager.GetPlayerFoodForCurrentDay());
            }

            if (_playerPresenter == null)
            {
                var playerView = FindObjectOfType<PlayerView>();
                _playerPresenter = new PlayerPresenter(_player, playerView);
            }
            
            _player.PlayerTurnEndEvent.AddListener(HandleEnemiesTurn);
            _player.PlayerReachedExitEvent.AddListener(HandlePlayerReachedExit);
            _player.PlayerDeadEvent.AddListener(GameOver);
            _player.PlayerCollisionEvent.AddListener(HandlePlayerCollision);
        }

        public void HandleEnemiesTurn()
        {
            playersTurn = false;
            _gameManagerPresenter.ShowCurrentTurn("Enemy");
            TryMoveEnemies();
        }

        private void HandlePlayerReachedExit()
        {
            IncreaseDay();
            SavePlayerFood();
            StartCoroutine(LoadNextLevel());
        }

        private void IncreaseDay()
        {
            _levelManager.CurrentDay++;
        }

        private void SavePlayerFood()
        {
            StaticLevelManager.CurrentPlayerFood = _player.Food;
        }

        private void HandlePlayerCollision(string collisionObjectName)
        {
            if (collisionObjectName == "Wall" && _player.IsInTheSamePosition())
            {
                _inMemoryCommandLogger.RemoveLastLoggedCommand();
            }
        }

        private IEnumerator LoadNextLevel()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadSceneAsync("Main");
        }

        private void InitPlayerGhost()
        {
            if (_playerGhost == null)
            {
                _playerGhost = FindObjectOfType<PlayerGhost>();
                if (_playerGhost != null)
                {
                    _playerGhost.Init();
                }
            }
        }

        private void InitGhostManager()
        {
            _ghostManager = FindObjectOfType<GhostManager>();
            if (_ghostManager != null)
            {
                _ghostManager.Init(_inMemoryCommandLogger, _playerGhost);
            }
            
            var ghostManagerView = GameObject.Find("GhostManagerView").GetComponent<GhostManagerView>(); 
            _ghostManagerPresenter = new GhostManagerPresenter(_ghostManager, ghostManagerView, _levelManager);
        }

        public void GameOver()
        {
            _gameManagerPresenter.ShowGameOver(_levelManager.CurrentDay);
            _levelManager.CurrentDay = 1;
            enabled = false;
        }

        private void InitBoard()
        {
            if (_boardManager == null)
            {
                _boardManager = GetComponent<BoardManager>();
            }
            _boardManager.SetupScene(_levelManager.CurrentDay);
        }

        private void InitEnemies()
        {
            _enemyManager = new EnemyManager(_boardManager.InstantiatedEnemies);
        }

        private void InitTimer()
        {
            _timer = new UnityTimer();
        }

        private void InitUi()
        {
            var gameManagerView = GameObject.Find("LevelImage").GetComponent<GameManagerView>(); 
            _gameManagerPresenter = new GameManagerPresenter(this, gameManagerView, _timer, _levelManager);
            _gameManagerPresenter.ShowCurrentDay(_levelManager.CurrentDay);
        }

        private void InitCommandLogger()
        {
            _inMemoryCommandLogger = new InMemoryCommandLogger(_levelManager);
            _consoleCommandLogger = new ConsoleCommandLogger(_timer);
            var commandsView = GameObject.Find("CommandsView").GetComponent<CommandsView>();
            _commandsPresenter = new CommandsPresenter(commandsView);
        }

        private void TryMoveEnemies()
        {
            StartCoroutine(MoveEnemies());
        }

        private IEnumerator MoveEnemies()
        {
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
            enemiesMoving = false;
            
            playersTurn = true;
            _gameManagerPresenter.ShowCurrentTurn("Player");
        }

        public bool IsPlayersTurn()
        {
            return playersTurn;
        }

    }
}