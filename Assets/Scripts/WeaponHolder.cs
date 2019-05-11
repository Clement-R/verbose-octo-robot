using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public Action<WeaponData> OnWeaponChange;

    public WeaponData WeaponData;

    [SerializeField] private SpriteRenderer handleSprite;
    [SerializeField] private SpriteRenderer guardSprite;
    [SerializeField] private SpriteRenderer bladeSprite;

    void Start()
    {
        if (WeaponData != null)
        {
            UpdateWeapon(WeaponData);
        }
    }

    public bool PickUpWeapon(WeaponData w)
    {
        WeaponData = w;
        UpdateWeapon(WeaponData);
        return true;
    }

    public void UpdateWeapon(WeaponData w)
    {
        handleSprite.sprite = w.handle;
        guardSprite.sprite = w.guard;
        bladeSprite.sprite = w.blade;

        OnWeaponChange?.Invoke(w);
    }

    //private void OnValidate()
    //{
    //    if (WeaponData != null)
    //    {
    //        UpdateWeapon(WeaponData);
    //    }
    //    else
    //    {
    //        handleSprite.sprite = guardSprite.sprite = bladeSprite.sprite = null;
    //    }
    //}
}
