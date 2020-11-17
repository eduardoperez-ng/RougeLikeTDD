namespace Completed.Interfaces
{
    public interface IGameManager
    {
        void Init();
        void GameOver();
        void HandleEnemiesTurn();
        bool IsPlayersTurn();
    }
}