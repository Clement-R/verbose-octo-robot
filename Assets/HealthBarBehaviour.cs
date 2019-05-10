using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarBehaviour : MonoBehaviour
{
    [SerializeField] private SpriteRenderer m_lifeBar;
    [SerializeField] private SpriteRenderer m_backgroundBar;

    private HealthBehaviour m_healthBehaviour = null;

    private void Awake()
    {
        m_healthBehaviour = GetComponentInParent<HealthBehaviour>();
        UpdateLife();

        m_healthBehaviour.OnHit += (GameObject p_attacker, int p_amount) => { UpdateLife(); };
        m_healthBehaviour.OnHeal += (int p_amount) => { UpdateLife(); };
    }

    private void UpdateLife()
    {
        Vector3 scale = m_lifeBar.gameObject.transform.localScale;
        float ratio = ((float)m_healthBehaviour.Life) / m_healthBehaviour.MaxLife;

        Debug.Log(m_healthBehaviour.Life + " / " + m_healthBehaviour.MaxLife + " || " + ratio);

        m_lifeBar.gameObject.transform.localScale = new Vector3(ratio, scale.y, scale.z);
    }
}
