using Completed.Interfaces;
using Completed.View;

namespace Completed.Presenter
{
    public class GameManagerPresenter
    {
        private GameManagerView _gameManagerView;
        private GameManager _gameManager;
        
        public GameManagerPresenter(GameManager gameManager, GameManagerView gameManagerView, 
            ITimer timer, ILevelManager levelManager)
        {
            _gameManager = gameManager;
            _gameManagerView = gameManagerView;
            _gameManagerView.Init(timer, levelManager);
        }

        public void ShowCurrentDay(int currentDay)
        {
            _gameManagerView.ShowCurrentDay(currentDay);
        }

        public void ShowGameOver(int currentDay)
        {
            _gameManagerView.ShowGameOver(currentDay);
        }

        public void ShowElapsedTime()
        {
            _gameManagerView.ShowElapsedTime();
        }

        public void ShowCurrentTurn(string gameActor)
        {
            _gameManagerView.ShowTurn(gameActor);
        }
    }
}