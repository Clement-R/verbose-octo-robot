using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableHealthItem : PickableItem
{
    [SerializeField] private int m_healAmount;

    protected override void OnEnter(Collider2D p_collision)
    {
        HealthBehaviour healthSystem = p_collision.gameObject.GetComponent<HealthBehaviour>();
        if (healthSystem)
            healthSystem.Heal(m_healAmount);

        Destroy(gameObject);
    }

    protected override void OnExit(Collider2D p_collision)
    {
    }

    protected override void OnStay(Collider2D p_collision)
    {
    }
}
