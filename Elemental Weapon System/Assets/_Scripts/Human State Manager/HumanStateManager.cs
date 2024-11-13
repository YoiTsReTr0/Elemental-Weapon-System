using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.Events;
using Elemental.WeaponSystem;

namespace Elemental.Main
{
    public class HumanStateManager : UnitHealth
    {
        #region State Manager

        public NormalState NormalState = new();
        public ShootingState ShootingState = new();
        public DeathState DeathState = new();

        #endregion


        #region Local + protected Variables

        private HumanBaseState CurrentState; // change protection level as required

        private WeaponsAction _currWeapon;
        protected List<WeaponsAction> _userWeaponList = new();
        protected WeaponSlotType _currentWeaponSlot;
        [SerializeField] protected bool _shootBtnDown;

        [SerializeField] private DefaultMeleeScript _defaultMeleeWeapon;

        protected Coroutine _changeWeaponCoroutine;

        private bool _isReloading;
        private bool _gameInitOnce;

        [SerializeField] private Transform _weaponSpawnPosi;

        #endregion

        #region Public Variables

        public Animator _animator;
        public Dictionary<WeaponType, string> _weaponStanceAnimDict;
        public Dictionary<WeaponType, string> _weaponReloadAnimDict;

        public bool IsAttacking = false; // protection public to match the state match requirements

        public bool ShootBtnDown
        {
            get => _shootBtnDown;
            set => _shootBtnDown = value;
        }

        #endregion


        #region Unity Events

        public UnityEvent<WeaponsAction> PickupWeaponEvent;
        public UnityEvent WeaponChangeEvent; // Manages all Weapon changes methods as per requirement
        public UnityEvent ShootWeaponEvent; // Called using user inputs, Calls shooting methods

        #endregion


        #region Pre-requisites

        /// <summary>
        /// Return Weapon slot by number as passing parameter
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private WeaponSlotType GetWeaponSlotByNum(int num)
        {
            switch (num)
            {
                case 0: return WeaponSlotType.Primary;
                case 1: return WeaponSlotType.Secondary;
                case 2: return WeaponSlotType.Sidearm;
                case 3: return WeaponSlotType.Melee;
            }

            return WeaponSlotType.Primary;
        }

        private int GetNumByWeaponSlot(WeaponSlotType slot)
        {
            switch (slot)
            {
                case WeaponSlotType.Primary: return 0;
                case WeaponSlotType.Secondary: return 1;
                case WeaponSlotType.Sidearm: return 2;
                case WeaponSlotType.Melee: return 3;
            }

            return 0;
        }

        private WeaponSlotType GetAvailableSlot(WeaponsAction weapon)
        {
            // check free slot then add to the empty one. Working as intended
            foreach (WeaponSlotType slot in Enum.GetValues(typeof(WeaponSlotType)))
            {
                bool isSlotUsed = _userWeaponList.Any(w => w.WeaponSlotPosition == slot);

                if (!isSlotUsed && weapon.WeaponSlotTypesPossible.Contains(slot))
                {
                    return slot;
                }
            }

            // Out of foreach loop meaning all slots are occupied
            // now compare with _currWeapon if the weapon has one of the slot as _currWeapon.WeaponSlotPosition. If yes then replace this weapon.
            // if weapon doesnt have that slot, example i have AR and wanna pick up Sidearm. or i have melee slot equipped but no AR or sidearm can fit here then replace with the first possible slot

            foreach (var aSlot in weapon.WeaponSlotTypesPossible)
            {
                WeaponsAction toReplaceWeapon;

                if (weapon.WeaponSlotTypesPossible.Contains(_currentWeaponSlot))
                    toReplaceWeapon = _currWeapon;
                else
                    toReplaceWeapon = _userWeaponList.Find(w => w.WeaponSlotPosition == aSlot);

                if (toReplaceWeapon)
                {
                    _userWeaponList.Remove(toReplaceWeapon);
                    return toReplaceWeapon.WeaponSlotPosition;
                }
            }

            return default;
        }

        /// <summary>
        /// Update Current Weapon data and manage events. Update Current Weapon Slot before this method is called
        /// </summary>
        void UpdateWeaponBySlot()
        {
            WeaponsAction newWeapon = _userWeaponList.Find(x => x.WeaponSlotPosition == _currentWeaponSlot);

            if (newWeapon)
            {
                _currWeapon = newWeapon;

                _currWeapon.Initialize();
                _currWeapon.WeaponUser = this;


                WeaponChangeEvent?.Invoke();
            }

            else
            {
                StopCoroutine(_changeWeaponCoroutine);
                _changeWeaponCoroutine = null;
            }
        }

        private void ManageReloadBool(bool reload)
        {
            _isReloading = reload;
        }

        #endregion

        private void Start()
        {
            // Animation Dictionary initialization
            {
                _weaponStanceAnimDict = new()
                {
                    { WeaponType.DefaultMelee, "MeleeStance" },
                    { WeaponType.AssaultRifle, "RifleStance" },
                    { WeaponType.Shotgun, "ShotgunStance" },
                    { WeaponType.Pistol, "PistolStance" }
                };

                _weaponReloadAnimDict = new()
                {
                    { WeaponType.AssaultRifle, "RifleReload" },
                    { WeaponType.Shotgun, "ShotgunReload" },
                    { WeaponType.Pistol, "PistolReload" }
                };
            }

            _animator = GetComponent<Animator>();
            //_currentWeaponSlot = WeaponSlotType.Melee;

            CurrentState = NormalState;
            CurrentState.EnterState(this);

            PickupWeaponEvent.AddListener((WeaponsAction weapon) => PickupWeapon(weapon));

            ShootWeaponEvent.AddListener(() =>
            {
                _currWeapon.RunShootAction();
                // Expecting animations (better to be controlled from weapon script itself)
            });
            WeaponChangeEvent.AddListener(() =>
            {
                _currWeapon.OnEquipWeaponEvent?.Invoke();
                Debug.Log("Change Weapon to " + _currWeapon.WeaponSlotPosition);
            });


            Invoke(nameof(InitializePlayer), 0.35f);
        }


        protected void Update()
        {
            CurrentState.UpdateFunc(this);

            if (ShootBtnDown)
                ShootEquippedWeapon();
        }


        public void SwitchState(HumanBaseState state)
        {
            CurrentState = state;
            CurrentState.EnterState(this);
        }


        private void InitializePlayer()
        {
            //Add melee as default weapon
            /*_defaultMeleeWeapon.WeaponSlotPosition = WeaponSlotType.Melee;

            _userWeaponList.Add(_defaultMeleeWeapon);
            _currWeapon = _defaultMeleeWeapon;
            _currentWeaponSlot = WeaponSlotType.Melee;

            UpdateWeaponBySlot();
            */

            PickupWeaponEvent?.Invoke(_defaultMeleeWeapon);
            ChangeWeapon(3);
        }

        /// <summary>
        /// Call to add weapon to player's inventory or replace with existing weapon 
        /// </summary>
        /// <param name="weapon">Weapon to be added or replaced with</param>
        private void PickupWeapon(WeaponsAction weapon)
        {
            weapon.OnPickUpWeaponEvent?.Invoke(GetAvailableSlot(weapon));

            _userWeaponList.Add(weapon);

            weapon.transform.SetParent(_weaponSpawnPosi);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
            weapon.gameObject.SetActive(false);

            if (weapon.WeaponSlotPosition == _currentWeaponSlot)
                ChangeWeapon(GetNumByWeaponSlot(_currentWeaponSlot), true);

            weapon.OnReloadWeaponStartEvent.AddListener(() => ManageReloadBool(true));
        }


        #region Controls and relative functions

        /// <summary>
        /// Sets ShootBtnDown bool to true for basics
        /// </summary>
        protected void CurrWeaponPullTrigger()
        {
            ShootBtnDown = true;
        }

        /// <summary>
        /// Sets ShootBtnDown bool to false for basics
        /// </summary>
        protected void CurrWeaponReleaseTrigger()
        {
            ShootBtnDown = false;
        }


        /// <summary>
        /// Additional method for shooting. Attach to any button or key bindings to trigger shooting and remove the ShootBtnDown bool dependent code from update
        /// </summary>
        protected void ShootEquippedWeapon()
        {
            IsAttacking = true;
            // Is Attacking may also be managed by other factors like interruptions during shooting, etc. 


            SwitchState(ShootingState);

            ShootWeaponEvent?.Invoke();
        }

        /// <summary>
        /// Attach to any button or key bindings to trigger reload
        /// </summary>
        protected void ReloadEquippedWeapon()
        {
            IsAttacking = false;
            // isAttacking bool purely based on future game design

            _currWeapon.OnReloadWeaponStartEvent?.Invoke();
        }

        /// <summary>
        /// Manages switching weapons FROM INVENTORY
        /// Attach to Weapon Change key binds
        /// Pressing 1 (considering num 1 is assigned to key 1) will equip the Primary weapon.
        /// Pressing 2 will equip the Secondary weapon.
        /// Pressing 3 will equip the Sidearm.
        /// </summary>
        protected void ChangeWeapon(int num, bool replaceWeapon = false)
        {
            if (_changeWeaponCoroutine != null)
                return;

            if (_currWeapon)
            {
                _currWeapon.OnWeaponChangeEvent?.Invoke();
                _currWeapon.gameObject.SetActive(false);
            }

            WeaponSlotType newWeaponSlot = GetWeaponSlotByNum(num);

            if (!replaceWeapon && newWeaponSlot == _currentWeaponSlot ||
                !_userWeaponList.Any(w => w.WeaponSlotPosition == newWeaponSlot))
                return;

            WeaponSlotType lastSlot = _currentWeaponSlot;

            _currentWeaponSlot = newWeaponSlot;
            UpdateWeaponBySlot();

            Debug.Log("Slot Changed to " + _currentWeaponSlot.ToString());

            _currWeapon.gameObject.SetActive(true);

            if (!_gameInitOnce)
            {
                _gameInitOnce = true;
                _animator.SetTrigger(_weaponStanceAnimDict[_currWeapon._weaponType]);
                return;
            }

            _changeWeaponCoroutine = StartCoroutine(RunChangeWeapon(lastSlot));
        }

        #endregion

        #region Helper Functions

        IEnumerator RunChangeWeapon(WeaponSlotType lastSlot)
        {
            if (lastSlot != WeaponSlotType.Melee)
                _animator.SetTrigger(_weaponStanceAnimDict[WeaponType.DefaultMelee]);

            // Temporary time value assignment, depends on game design. Counter Strike has instant weapon change, Call Of Duty has some delay in change.
            yield return new WaitForSeconds(0.001f);

            if (_currentWeaponSlot != WeaponSlotType.Melee)
                _animator.SetTrigger(_weaponStanceAnimDict[_currWeapon._weaponType]);

            _changeWeaponCoroutine = null;
            yield return null;
        }

        #endregion
    }
}