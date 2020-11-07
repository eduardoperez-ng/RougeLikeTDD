namespace Completed
{
    public static class StaticLevelManager
    {
        private const int PlayerStartingFoodPoints = 100;
        public static int CurrentDay { get; set; }
        public static int CurrentPlayerFood { get; set; }

        public static int GetPlayerFoodForCurrentDay()
        {
            return CurrentDay == 1 ? PlayerStartingFoodPoints : CurrentPlayerFood;
        }
    }
}