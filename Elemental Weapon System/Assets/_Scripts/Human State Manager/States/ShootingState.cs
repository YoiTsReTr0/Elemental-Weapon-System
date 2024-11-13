namespace Elemental.Main
{
    public class ShootingState : HumanBaseState
    {
        public override void EnterState(HumanStateManager hManager)
        {
            // manage animations for shooting state, dynamic events etc
        }


        public override void UpdateFunc(HumanStateManager hManager)
        {
            if (!hManager.IsAttacking)
                hManager.SwitchState(hManager.NormalState);

            else if (hManager.IsDead)
                hManager.SwitchState(hManager.DeathState);
        }
    }
}