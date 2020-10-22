using UnityEngine;

namespace Completed
{
    public interface GameActor
    {
        bool Move(int xDir, int yDir, out RaycastHit2D hit);
        void AttemptMove<T>(int xDir, int yDir) where T : Component;
    }
}