using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour
{
    public Action OnDeath;
    public Action<GameObject, int> OnHit;

    public int Life { get { return m_life; } }

    private int m_life = 200;
    private int m_maxLife = 200;

    public void Hit(GameObject p_attacker, int p_amount)
    {
        m_life -= p_amount;

        if (m_life <= 0)
            Die();

        OnHit?.Invoke(p_attacker, p_amount);

        Debug.Log(gameObject.name + " : " + Life);
    }

    public void Heal(int p_amount)
    {
        m_life = Mathf.Clamp(m_life + p_amount, 0, m_maxLife);

        Debug.Log(gameObject.name + " : " + Life);
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
