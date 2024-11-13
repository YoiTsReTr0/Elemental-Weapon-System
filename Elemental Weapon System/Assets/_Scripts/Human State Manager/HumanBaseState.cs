namespace Elemental.Main
{
    public abstract class HumanBaseState
    {
        public abstract void EnterState(HumanStateManager hManager);
        public abstract void UpdateFunc(HumanStateManager hManager);
    }
}