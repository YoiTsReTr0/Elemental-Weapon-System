using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using Elemental.Main;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Elemental.WeaponSystem
{
    public class WeaponsAction : MonoBehaviour
    {
        #region Editor Vars

        [SerializeField] private WeaponTypeSO WeaponData;
        [SerializeField] protected Transform _weaponEndPT;

        #endregion

        #region Public Variables

        public WeaponSlotType WeaponSlotPosition;
        public WeaponType _weaponType;
        public List<WeaponSlotType> WeaponSlotTypesPossible = new();
        public MeshRenderer[] WeaponMeshRenderers;

        public HumanStateManager WeaponUser
        {
            get => _weaponUser;
            set => _weaponUser = value;
        }

        #endregion

        #region Local Variables + protected

        private WeaponPowerType _weaponPowerType;

        private ElementalUIManager _uiManager;
        private ElementalGameManager _gameManager;

        private WeaponOwner _whoIsOwner = WeaponOwner.OnGround;

        private int _currAmmoCount;
        private bool _isReloading = false;
        protected bool _triggerPressed;

        protected bool _fireRateDelayBool = false;
        protected int _layersToHit = 0;

        protected HumanStateManager _weaponUser;

        protected Coroutine _reloadCoroutine;

        #endregion

        #region Weapon Data Vars (can be view in editor using [SerializeField]

        //[Space(15f), Header("Weapon Data (Local storage)"), Space(10)]

        #region If !Melee

        [Space(5)] private string _weaponName;
        private Sprite _weaponImage;


        private ProjectileType _projectileType;
        protected Projectile _projectileModel;

        private int _currMaxAmmoSize;
        private float _fireRate;
        private float _reloadTime;
        private float _attackRange;

        protected int _projectilesSpawn;
        protected float _damagePerProjectile;
        protected float _areaOfImpactRadius;
        protected float _projectileSpeed;

        private WeaponFireMode _currWeaponFireMode;

        #endregion

        #region If Melee Vars

        private float _hitRate; // Hit rate can be controlled through animations or manually using this property
        private float _damagePerHit;

        #endregion

        #endregion

        #region Unity Events

        /// <summary>
        /// Called after weapon is picked
        /// </summary>
        [HideInInspector] public UnityEvent<WeaponSlotType> OnPickUpWeaponEvent;

        /// <summary>
        /// Called after weapon is changed - expect UI updates
        /// </summary>
        [HideInInspector] public UnityEvent OnEquipWeaponEvent;

        /// <summary>
        /// Called to start reload function. Currently manages entire reload flow, start and end
        /// </summary>
        [HideInInspector] public UnityEvent OnReloadWeaponStartEvent;

        [HideInInspector] public UnityEvent OnReloadWeaponOverEvent;
        [HideInInspector] public UnityEvent OnWeaponChangeEvent;

        #endregion

        #region Pre-requisites

        private Material GetMatByPower()
        {
            switch (_weaponPowerType)
            {
                case WeaponPowerType.Lightning: return _gameManager.LightningMat;
                case WeaponPowerType.Ice: return _gameManager.IceMat;
                case WeaponPowerType.Fire: return _gameManager.FireMat;
                default: return _gameManager.FireMat;
            }
        }

        private Projectile GetProjectileByType()
        {
            switch (_projectileType)
            {
                case ProjectileType._9mm: return _gameManager._9mm;
                case ProjectileType._7pt62: return _gameManager._7pt62;
                case ProjectileType._5pt56: return _gameManager._5pt56;
                case ProjectileType._buckshot: return _gameManager._buckshot;
                default: return _gameManager._9mm;
            }
        }

        #endregion

        private void Awake()
        {
            WeaponDataUpdate(WeaponData);
        }

        private void Start()
        {
            _layersToHit = (1 << LayerMask.NameToLayer("ShootableEntity")) |
                           (1 << LayerMask.NameToLayer("ObstacleEnv"));

            OnPickUpWeaponEvent.AddListener((WeaponSlotType slot) => { PickUpWeapon(slot); });

            OnEquipWeaponEvent.AddListener(() =>
            {
                _uiManager.OnWeaponEquipUIUpdateEvent?.Invoke(WeaponSlotPosition, _weaponName,
                    _currAmmoCount, _currMaxAmmoSize);
                _whoIsOwner = WeaponOwner.Bearer;

                Debug.Log("Weapon Changed to " + _weaponType.ToString());
            });

            OnReloadWeaponStartEvent.AddListener(() =>
            {
                if (!_isReloading)
                    _reloadCoroutine = StartCoroutine(Reload(_reloadTime));
            });

            OnReloadWeaponOverEvent.AddListener(() =>
            {
                _currAmmoCount = _currMaxAmmoSize;
                _isReloading = false;

                Debug.Log("Reload Complete");

                _uiManager.OnReloadEndUIUpdateEvent?.Invoke(_currAmmoCount, _currMaxAmmoSize);
            });

            OnWeaponChangeEvent.AddListener(() =>
            {
                if (_reloadCoroutine != null)
                {
                    Debug.Log("Weapon Change during reload");
                    StopCoroutine(_reloadCoroutine);
                    _reloadCoroutine = null;

                    _isReloading = false;
                }
            });

            _uiManager = ElementalUIManager.Instance;
            _gameManager = ElementalGameManager.instance;
        }

        protected delegate void ShootWeaponDelegate();

        protected ShootWeaponDelegate ShootWeapon;


        private void WeaponDataUpdate(WeaponTypeSO SO_weaponData) // Local
        {
            _weaponType = SO_weaponData.WeaponType;
            _weaponPowerType = SO_weaponData.WeaponPowerType;
            WeaponSlotTypesPossible = SO_weaponData.WeaponSlotTypes;
            _weaponName = SO_weaponData.WeaponName;

            _weaponImage = SO_weaponData.WeaponImage;

            if (SO_weaponData.WeaponType == WeaponType.DefaultMelee)
            {
                _hitRate = SO_weaponData.HitRate;
                _damagePerHit = SO_weaponData.DamagePerHit;
            }

            else
            {
                _projectileType = SO_weaponData.ProjectileType;

                _attackRange = SO_weaponData.AttackRange;
                _currMaxAmmoSize = SO_weaponData.MagezineSize;
                _fireRate = SO_weaponData.FireRate;
                _reloadTime = SO_weaponData.ReloadTime;

                _projectilesSpawn = SO_weaponData.ProjectilesSpawnCount;
                _damagePerProjectile = SO_weaponData.DamagePerProjectile;
                _areaOfImpactRadius = SO_weaponData.AreaOfImpactRadius;
                _projectileSpeed = SO_weaponData.ProjectileSpeed;

                _currWeaponFireMode = SO_weaponData.WeaponFireMode;

                _currAmmoCount = _currMaxAmmoSize;
            }
        }

        public virtual void Initialize()
        {
            if (WeaponSlotPosition != WeaponSlotType.Melee)
            {
                foreach (var x in WeaponMeshRenderers)
                {
                    x.material = GetMatByPower();
                }

                _projectileModel = GetProjectileByType();
                foreach (var x in _projectileModel._meshRenderers)
                {
                    x.material = GetMatByPower();
                }
            }
        }

        private void PickUpWeapon(WeaponSlotType slot)
        {
            WeaponSlotPosition = slot;
            _uiManager.OnWeaponPickupUIUpdateEvent?.Invoke(_weaponImage, WeaponSlotPosition);
        }


        /// <summary>
        /// Calling this main function to call Shoot Weapon Delegate thus firing up the entire shooting system
        /// </summary>
        public void RunShootAction()
        {
            // Trigger false managed in the weapons itself. No dependencies here
            _triggerPressed = true;
            ShootWeapon?.Invoke();
        }


        /// <summary>
        /// Shooting function for each weapon
        /// </summary>
        public virtual bool ShootWeaponBool()
        {
            if (_isReloading)
                return false;

            if (_currWeaponFireMode != WeaponFireMode.FullAuto && _weaponUser.ShootBtnDown ||
                _weaponType == WeaponType.DefaultMelee && _weaponUser.ShootBtnDown)
                _weaponUser.ShootBtnDown = false;

            else if (_currWeaponFireMode != WeaponFireMode.FullAuto && !_weaponUser.ShootBtnDown ||
                     _weaponType == WeaponType.DefaultMelee && !_weaponUser.ShootBtnDown)
                return false;

            else if (_currWeaponFireMode == WeaponFireMode.FullAuto && !_weaponUser.ShootBtnDown)
                return false;

            if (_fireRateDelayBool)
            {
                Debug.Log("Fire Rate Delay. Shoot Weapon Return");
                return false;
            }

            if (_weaponType == WeaponType.DefaultMelee)
            {
                // Add functionality as per requirement, depends on game design
                _uiManager.OnShootUIUpdateEvent?.Invoke(0, 0);
            }

            else
            {
                if (_currAmmoCount <= 0)
                {
                    Debug.Log("Out Of Ammo");

                    //TriggerPressed = false; Depends on game design

                    if (!_isReloading)
                        _reloadCoroutine = StartCoroutine(Reload(_reloadTime));

                    //weaponUser.OnShootUIUpdate?.Invoke();

                    if (_currWeaponFireMode == WeaponFireMode.FullAuto && _weaponUser.ShootBtnDown)
                        _weaponUser.ShootBtnDown = false;

                    return false;
                }

                _currAmmoCount--;


                _uiManager.OnShootUIUpdateEvent?.Invoke(_currAmmoCount, _currMaxAmmoSize);
            }

            _weaponUser._animator.SetTrigger("Shoot");
            return true;
        }


        /// <summary>
        /// Simple Coroutine for Single Shot Weapons
        /// </summary>
        /// <returns></returns>
        protected IEnumerator ShootingDelay()
        {
            if (_weaponType == WeaponType.DefaultMelee)
                yield return new WaitForSeconds(_hitRate);

            else
                yield return new WaitForSeconds(_fireRate);


            _fireRateDelayBool = false;
        }


        /// <summary>
        /// Coroutine for Auto Weapons automatic shooting simulation
        /// </summary>
        /// <param name="ShootAutoWeapon"></param>
        /// <returns></returns>
        protected IEnumerator ShootingDelay(ShootWeaponDelegate ShootAutoWeapon)
        {
            yield return new WaitForSeconds(_fireRate);

            _fireRateDelayBool = false;


            ShootAutoWeapon?.Invoke();
        }


        // ReSharper disable Unity.PerformanceAnalysis
        /// <summary>
        /// Coroutine for Reload time management
        /// </summary>
        /// <param name="ReloadTime"></param>
        /// <returns></returns>
        public IEnumerator Reload(float ReloadTime)
        {
            if (_currAmmoCount == _currMaxAmmoSize || _isReloading)
            {
                yield break;
            }

            _triggerPressed = false;

            ReloadTime = CalculateReloadTime(ReloadTime);

            Debug.Log("Reload Started");

            _uiManager.OnReloadStartUIUpdateEvent?.Invoke();

            _isReloading = true;

            _weaponUser._animator.SetTrigger(_weaponUser._weaponReloadAnimDict[_weaponType]);

            yield return new WaitForSeconds(ReloadTime);

            OnReloadWeaponOverEvent?.Invoke();
        }

        private float CalculateReloadTime(float time)
        {
            // Variable reload time, disabled for this demonstration 
            /*if (_weaponType == WeaponType.Shotgun)
            {
                time -= (1.25f * _currAmmoCount);
            }

            else if (_weaponType == WeaponType.AssaultRifle || _weaponType == WeaponType.Pistol)
            {
                if (_currAmmoCount > 0)
                    time *= 0.8f;
            }*/

            return time;
        }

        public void CancelReload()
        {
            if (_reloadCoroutine != null)
            {
                StopCoroutine(_reloadCoroutine);

                _reloadCoroutine = null;
            }

            _isReloading = false;
        }


        // Each weapon can have their own reload functions depending on the size of the project and diversity of the weapons. Usage of WeaponType will still be available and modify-able as per requirement.
    }
}