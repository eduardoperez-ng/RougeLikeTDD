namespace Completed.Commands
{
    [System.Serializable]
    public class MoveRightCommand : Command
    {
        public override void Execute(GameActor gameActor)
        {
            gameActor.AttemptMove<Wall>(1, 0);
        }
        
        public override string ToString()
        {
            return "right";
        }
    }
}