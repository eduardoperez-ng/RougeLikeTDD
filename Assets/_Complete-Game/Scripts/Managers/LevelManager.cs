using Completed.Interfaces;

namespace Completed
{
    public class LevelManager : ILevelManager
    {
        public int CurrentDay
        {
            get => StaticLevelManager.CurrentDay;
            set => StaticLevelManager.CurrentDay = value;
        }
    }
}