using System.Collections;

namespace Completed.Interfaces
{
    public interface IGameManager
    {
        void Init();
        void HideLevelImage();
        void AddEnemyToList(Enemy script);
        void GameOver();
        IEnumerator MoveEnemies();
        void EndPlayerTurn();
        bool IsPlayersTurn();
    }
}