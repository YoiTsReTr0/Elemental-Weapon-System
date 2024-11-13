using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Elemental.WeaponSystem
{
    public class PistolClassWeapon : WeaponsAction
    {
        private Vector3 _moveDirection;

        public override void Initialize()
        {
            base.Initialize();
            base.ShootWeapon += ShootThisWeapon;
        }

        private Vector3 ProjectileSpread()
        {
            Vector3 returnValue = new Vector3(_moveDirection.x + Random.Range(-0.1f, 0.1f),
                _moveDirection.y + Random.Range(-0.1f, 0.1f),
                _moveDirection.z);

            return returnValue;
        }

        private void ShootThisWeapon()
        {
            if (!base.ShootWeaponBool())
                return;

            Vector3 direction = (_weaponEndPT.transform.position - transform.position).normalized;
            Projectile projectile = Instantiate(_projectileModel, _weaponEndPT.position,
                Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0));

            projectile.FeedData(_damagePerProjectile, _areaOfImpactRadius, _projectileSpeed,
                direction + ProjectileSpread(), _layersToHit,
                _weaponEndPT.transform.position);

            _triggerPressed = false;
            // single shoot automatically unpressed trigger so user has to re-press the key binding for shooting

            Debug.Log("Pistol attacking");

            _fireRateDelayBool = true;

            StartCoroutine(base.ShootingDelay());
        }
    }
}