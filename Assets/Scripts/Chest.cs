using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{

    public bool isOpen = true;
    [SerializeField] private Sprite openChest;
    [SerializeField] private Sprite closedChest;
    private SpriteRenderer spriteRenderer;

    public WeaponData m_Loot;

    [SerializeField] private SpriteRenderer handleSprite;
    [SerializeField] private SpriteRenderer guardSprite;
    [SerializeField] private SpriteRenderer bladeSprite;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            spriteRenderer.sprite = openChest;
        } else
        {
            spriteRenderer.sprite = closedChest;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isOpen)
        {
            if (other.GetComponent<PlayerCombat>() != null)
            {
                var player = other.GetComponent<PlayerCombat>();

                if (player.Weapon.PickUpWeapon(m_Loot))
                {
                    // Close the chest
                    m_Loot = null;
                    isOpen = false;
                    handleSprite.sprite = guardSprite.sprite = bladeSprite.sprite = null;
                }
                else
                {
                    // TODO: Is this useful ?
                }
            }
        }
    }

    public void UpdateWeapon(WeaponData w)
    {
        handleSprite.sprite = w.handle;
        guardSprite.sprite = w.guard;
        bladeSprite.sprite = w.blade;
    }

    private void OnValidate()
    {
        if (m_Loot != null)
        {
            UpdateWeapon(m_Loot);
        }
        else
        {
            handleSprite.sprite = guardSprite.sprite = bladeSprite.sprite = null;
        }
    }
}
