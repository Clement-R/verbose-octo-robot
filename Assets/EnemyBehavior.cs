using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private float m_knockBackStrength = 2f;
    private Rigidbody2D m_rb2d;

    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
    }

    public void Attack(GameObject p_attacker, int p_damages)
    {
        Debug.Log("Attacked!");

        Vector2 direction = new Vector2(0f, 0f);
        if (p_attacker.transform.position.x > transform.position.x)
            direction.x = -1f;
        else
            direction.x = 1f;

        StartCoroutine(_KnockBackEffect(direction));
    }

    private IEnumerator _KnockBackEffect(Vector2 p_direction)
    {
        m_rb2d.AddForce(transform.right * p_direction * m_knockBackStrength, ForceMode2D.Impulse);

        var material = GetComponent<SpriteRenderer>().material;

        material.SetColor("_BlinkColor", Color.white);
        material.SetFloat("_BlinkAmount", 1f);
        for (int i = 0; i < 5; i++)
        {
            yield return null;
        }

        material.SetColor("_BlinkColor", Color.red);
        material.SetFloat("_BlinkAmount", 0.5f);
        for (int i = 0; i < 10; i++)
        {
            yield return null;
        }

        material.SetFloat("_BlinkAmount", 0f);

        m_rb2d.velocity = Vector2.zero;
    }
}
