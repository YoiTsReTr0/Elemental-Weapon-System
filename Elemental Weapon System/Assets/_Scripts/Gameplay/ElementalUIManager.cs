using System;
using System.Collections;
using System.Collections.Generic;
using Elemental.Main;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class ElementalUIManager : MonoBehaviour
{
    public static ElementalUIManager Instance;

    #region Editor Variables

    [SerializeField] private TextMeshProUGUI _currAmmoCountText;
    [SerializeField] private TextMeshProUGUI _currWeaponNameText;
    [SerializeField] private List<WeaponUIContainer> _weaponSlotContainers;

    #endregion

    #region Local Variables

    #endregion

    #region Unity Events

    [Space(35), Header("Unity Events")] public UnityEvent<int, int> OnShootUIUpdateEvent;
    public UnityEvent OnReloadStartUIUpdateEvent;
    public UnityEvent<int, int> OnReloadEndUIUpdateEvent;
    public UnityEvent<WeaponSlotType, string, int, int> OnWeaponEquipUIUpdateEvent;
    public UnityEvent<Sprite, WeaponSlotType> OnWeaponPickupUIUpdateEvent;

    #endregion

    #region Pre-requisites

    private WeaponUIContainer GetWeaponContainerBySlot(WeaponSlotType slot)
    {
        switch (slot)
        {
            case WeaponSlotType.Primary: return _weaponSlotContainers[0];
            case WeaponSlotType.Secondary: return _weaponSlotContainers[1];
            case WeaponSlotType.Sidearm: return _weaponSlotContainers[2];
            case WeaponSlotType.Melee: return _weaponSlotContainers[3];
        }

        return null;
    }

    #endregion

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(gameObject);
    }

    private void Start()
    {
        OnShootUIUpdateEvent.AddListener((int _currAmmoCount, int _currMaxAmmoSize) =>
        {
            string currAmmo = _currAmmoCount == 0 ? "-" : _currAmmoCount.ToString();
            string maxAmmo = _currMaxAmmoSize == 0 ? "-" : _currMaxAmmoSize.ToString();
            _currAmmoCountText.text = $"{currAmmo} / {maxAmmo}";
        });

        OnReloadEndUIUpdateEvent.AddListener((int _currAmmoCount, int _currMaxAmmoSize) =>
        {
            string currAmmo = _currAmmoCount == 0 ? "-" : _currAmmoCount.ToString();
            string maxAmmo = _currMaxAmmoSize == 0 ? "-" : _currMaxAmmoSize.ToString();
            _currAmmoCountText.text = $"{currAmmo} / {maxAmmo}";
        });

        OnWeaponEquipUIUpdateEvent.AddListener((WeaponSlotType slot, string name, int _currAmmoCount,
            int _currMaxAmmoSize) =>
        {
            // Weapon selection/equip UI image setup
            {
                foreach (var x in _weaponSlotContainers)
                {
                    x.SelectionImage.gameObject.SetActive(false);
                }

                WeaponUIContainer slotContainer = GetWeaponContainerBySlot(slot);
                if (slotContainer != null)
                {
                    slotContainer.SelectionImage.gameObject.SetActive(true);
                }
            }


            _currWeaponNameText.text = name;

            string currAmmo = _currAmmoCount == 0 ? "-" : _currAmmoCount.ToString();
            string maxAmmo = _currMaxAmmoSize == 0 ? "-" : _currMaxAmmoSize.ToString();
            _currAmmoCountText.text = $"{currAmmo} / {maxAmmo}";
        });

        OnWeaponPickupUIUpdateEvent.AddListener((Sprite img, WeaponSlotType slot) =>
        {
            GetWeaponContainerBySlot(slot).WeaponImage.sprite = img;
            GetWeaponContainerBySlot(slot).WeaponImage.enabled = true;
        });
    }
}