namespace Inquisitor.State
{
    public abstract class AbstractInquisitorState
    {
        public abstract void Update(InquisitorController inquisitor);
        public abstract void EnterState(InquisitorController inquisitor);
        public abstract void ExitState(InquisitorController inquisitor);
    }
}