using Elemental.Main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// This class specifically handles the UI shoot button On Hold Events for player.
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonHeldDownShoot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private PlayerScript _player;

    private void Start()
    {
        _player = PlayerScript.Instance;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _player.UIShootEquippedWeaponDown();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _player.UIShootEquippedWeaponUp();
    }
}