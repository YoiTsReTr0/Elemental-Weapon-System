using UnityEngine;


namespace Elemental.WeaponSystem
{
    public class DefaultMeleeScript : WeaponsAction
    {
        public override void Initialize()
        {
            base.Initialize();
            base.ShootWeapon += ShootThisWeapon;
        }

        public void ShootThisWeapon()
        {
            if (!base.ShootWeaponBool())
                return;


            // Implement Melee animation or whatever type of melee functionality is chosen in Game Design


            _triggerPressed = false;
            // single shoot automatically unpress trigger so user has to re-press the key binding for shooting

            Debug.Log("Melee attacking");

            _fireRateDelayBool = true;

            StartCoroutine(base.ShootingDelay());
        }
    }
}