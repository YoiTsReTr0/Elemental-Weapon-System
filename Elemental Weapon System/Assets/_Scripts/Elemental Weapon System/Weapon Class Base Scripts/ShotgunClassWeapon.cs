using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Elemental.WeaponSystem
{
    public class ShotgunClassWeapon : WeaponsAction
    {
       private Vector3 _moveDirection;

        public override void Initialize()
        {
            base.Initialize();
            base.ShootWeapon += ShootThisWeapon;

            _moveDirection = (_weaponEndPT.transform.position - transform.position).normalized;
        }

        private Vector3 ProjectileSpread()
        {
            Vector3 returnValue = new Vector3(_moveDirection.x + Random.Range(-0.3f, 0.3f),
                _moveDirection.y + Random.Range(-0.3f, 0.3f),
                _moveDirection.z);

            return returnValue;
        }

        private void ShootThisWeapon()
        {
            if (!base.ShootWeaponBool())
                return;

            //Commented out due to no practical demonstration available

            for (int i = 0; i < base._projectilesSpawn; i++)
            {
                Projectile projectile = Instantiate(_projectileModel,
                    new(_weaponEndPT.position.x + +Random.Range(-0.169f, 0.169f),
                        _weaponEndPT.position.y + +Random.Range(-0.169f, 0.169f),
                        _weaponEndPT.position.z + +Random.Range(-0.169f, 0.169f)),
                    Quaternion.LookRotation(_moveDirection));

                projectile.FeedData(_damagePerProjectile, _areaOfImpactRadius, _projectileSpeed,
                    _moveDirection + ProjectileSpread(), _layersToHit,
                    _weaponEndPT.transform.position);
            }

            _triggerPressed = false;
            // single shoot automatically unpress trigger so user has to re-press the key binding for shooting

            Debug.Log("Shotgun attacking");

            _fireRateDelayBool = true;

            StartCoroutine(base.ShootingDelay());
        }
    }
}