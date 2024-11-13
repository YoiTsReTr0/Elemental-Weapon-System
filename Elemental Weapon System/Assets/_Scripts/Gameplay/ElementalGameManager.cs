using System;
using System.Collections;
using System.Collections.Generic;
using Elemental.WeaponSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Elemental.Main
{
    public class ElementalGameManager : MonoBehaviour
    {
        public static ElementalGameManager instance;

        #region Editor Variables

        [SerializeField] private HumanStateManager player;

        #endregion

        #region Local Variables

        #endregion

        #region Public Variables

        [Header("Materials")] public Material FireMat;
        public Material IceMat;
        public Material LightningMat;

        [Header("Projectile Models/Classes" +
                "")]
        public Projectile _9mm;
        public Projectile _7pt62;
        public Projectile _5pt56;
        public Projectile _buckshot;

        #endregion

        #region Unity Events

        #endregion

        #region Pre-requisites

        #endregion

        private void Awake()
        {
            if (instance == null)
                instance = this;

            else
                Destroy(gameObject);
        }

        public void TempAssignWeapon(WeaponsAction weapon)
        {
            player.PickupWeaponEvent?.Invoke(weapon);
        }
    }
}