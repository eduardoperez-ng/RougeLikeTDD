namespace Completed.Commands
{
    [System.Serializable]
    public class MoveUpCommand : Command
    {
        public override void Execute(GameActor gameActor)
        {
            gameActor.AttemptMove<Wall>(0, 1);
        }
    }
}