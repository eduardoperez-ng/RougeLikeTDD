using System.Collections;

namespace Completed.Interfaces
{
    public interface IGameManager
    {
        void Init();
        void HideLevelImage();
        void GameOver();
        IEnumerator MoveEnemies();
        void HandleEndPlayerTurn();
        bool IsPlayersTurn();
    }
}