using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Elemental.Main
{
    public class PlayerScript : HumanStateManager
    {
        // All pre-requisites are setup in the Human State Manager which will be the common script for both player and enemies. Specifically for Human behaviours capable of shooting weapons. 

        // This script will focus on the player controls and physics. Plus other responsibilities of the player script like, cutscene activities, objects interaction, Dialogue management and responses. 
        public static PlayerScript Instance;

        #region Editor Variables

        [SerializeField] private bool _mobileControl;

        #endregion

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);
        }

        private void Update()
        {
            base.Update();

            #region Player controls for Keyboard and Mouse

            if (!_mobileControl)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && !ShootBtnDown)
                {
                    CurrWeaponPullTrigger();
                }

                else if (Input.GetKeyUp(KeyCode.Mouse0) && ShootBtnDown)
                {
                    CurrWeaponReleaseTrigger();
                }

                else if (Input.GetKeyDown(KeyCode.R))
                {
                    ReloadEquippedWeapon();
                }

                // Following code is for weapon swap and direct method is much optimized than other clean coding methods for this
                else if (Input.GetKeyDown(KeyCode.Alpha1) && _changeWeaponCoroutine == null)
                {
                    ChangeWeapon(0);
                }

                else if (Input.GetKeyDown(KeyCode.Alpha2) && _changeWeaponCoroutine == null)
                {
                    ChangeWeapon(1);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) && _changeWeaponCoroutine == null)
                {
                    ChangeWeapon(2);
                }

                else if (Input.GetKeyDown(KeyCode.Alpha4) && _changeWeaponCoroutine == null)
                {
                    ChangeWeapon(3);
                }
            }

            #endregion
        }

        #region Player Controls for Mobile

        public void UIShootEquippedWeaponDown()
        {
            CurrWeaponPullTrigger();
        }

        public void UIShootEquippedWeaponUp()
        {
            CurrWeaponReleaseTrigger();
        }

        public void UIReloadEquippedWeapon()
        {
            ReloadEquippedWeapon();
        }

        public void UIEquipWeapon(int num)
        {
            ChangeWeapon(num);
        }

        #endregion

        // Add OnTriggerStay for Pick weapon function call, can be attached with the F key for Keyboard controls or UI buttons for mobile as per game design
    }
}