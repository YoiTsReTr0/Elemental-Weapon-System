namespace Elemental.Main
{
    public class NormalState : HumanBaseState
    {
        public override void EnterState(HumanStateManager hManager)
        {
            // manage all animations, dynamic events etc
        }


        public override void UpdateFunc(HumanStateManager hManager)
        {
            if (hManager.IsAttacking)
                hManager.SwitchState(hManager.ShootingState);

            else if (hManager.IsDead)
                hManager.SwitchState(hManager.DeathState);
        }
    }
}