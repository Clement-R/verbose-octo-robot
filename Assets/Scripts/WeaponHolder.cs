using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{

    public WeaponData weaponData;

    [SerializeField] private SpriteRenderer handleSprite;
    [SerializeField] private SpriteRenderer guardSprite;
    [SerializeField] private SpriteRenderer bladeSprite;

    void Start()
    {
        if (weaponData != null)
        {
            UpdateWeapon(weaponData);
        }
    }

    public bool PickUpWeapon(WeaponData w)
    {
        weaponData = w;
        UpdateWeapon(weaponData);
        return true;
    }

    public void UpdateWeapon(WeaponData w)
    {
        handleSprite.sprite = w.handle;
        guardSprite.sprite = w.guard;
        bladeSprite.sprite = w.blade;
    }

    private void OnValidate()
    {
        if (weaponData != null)
        {
            UpdateWeapon(weaponData);
        }
        else
        {
            handleSprite.sprite = guardSprite.sprite = bladeSprite.sprite = null;
        }
    }
}
