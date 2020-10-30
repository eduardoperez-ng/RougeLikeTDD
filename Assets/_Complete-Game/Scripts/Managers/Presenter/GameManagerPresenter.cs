using Completed.View;

namespace Completed.Presenter
{
    public class GameManagerPresenter
    {
        private GameManagerView _gameManagerView;
        private GameManager _gameManager;
        
        public GameManagerPresenter(GameManager gameManager, GameManagerView gameManagerView)
        {
            _gameManager = gameManager;
            _gameManagerView = gameManagerView;
            _gameManagerView.Init();
        }

        public void ShowCurrentDay(int currentDay)
        {
            _gameManagerView.ShowCurrentDay(currentDay);
        }

        public void ShowGameOver(int currentDay)
        {
            _gameManagerView.ShowGameOver(currentDay);
        }
    }
}