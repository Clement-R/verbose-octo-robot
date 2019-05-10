using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBehaviour : MonoBehaviour
{
    public Action OnDeath;
    public Action<GameObject, int> OnHit;
    public Action<int> OnHeal;

    public int Life { get { return m_life; } }
    public int MaxLife { get { return m_maxLife; } }

    [SerializeField] private FloatingText m_floatingText; 

    private int m_life = 200;
    private int m_maxLife = 200;
    private float m_height = 0f;

    private void Start()
    {
        m_height = GetComponent<SpriteRenderer>().size.y;
    }

    public void Hit(GameObject p_attacker, int p_amount)
    {
        m_life -= p_amount;

        if (m_life <= 0)
            Die();

        Vector2 pos = new Vector3(transform.position.x, transform.position.y + m_height);
        GameObject floatingText = Instantiate(m_floatingText.gameObject, pos, Quaternion.identity);
        floatingText.GetComponent<FloatingText>().Init(p_amount.ToString());

        OnHit?.Invoke(p_attacker, p_amount);
    }

    public void Heal(int p_amount)
    {
        m_life = Mathf.Clamp(m_life + p_amount, 0, m_maxLife);

        Vector2 pos = new Vector3(transform.position.x, transform.position.y + m_height);
        GameObject floatingText = Instantiate(m_floatingText.gameObject, pos, Quaternion.identity);
        floatingText.GetComponent<FloatingText>().Init(p_amount.ToString());

        OnHeal?.Invoke(p_amount);
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
