namespace Completed.Commands
{
    [System.Serializable]
    public class MoveDownCommand : Command
    {
        public override void Execute(GameActor gameActor)
        {
            gameActor.AttemptMove<Wall>(0, -1);
        }

        public override string ToString()
        {
            return "down";
        }
    }
}