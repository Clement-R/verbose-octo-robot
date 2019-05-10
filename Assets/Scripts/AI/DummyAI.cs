using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DummyAI : MonoBehaviour
{
    [SerializeField] private float m_safeDistance = 0.25f;

    private GameObject m_player;
    private CharacterController m_characterController;
    private SpriteRenderer m_sr;

    [SerializeField] protected float m_attackX = 1f;
    [SerializeField] protected float m_attackY = 0.25f;

    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_characterController = GetComponent<CharacterController>();
        m_sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Color color = Color.blue;

        if (m_player.transform.position.x > transform.position.x)
            m_characterController.FacingDirection = EDirection.LEFT;
        else
            m_characterController.FacingDirection = EDirection.RIGHT;

        if (m_characterController.FacingDirection == EDirection.LEFT && transform.rotation.y != 180f)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        else if (m_characterController.FacingDirection == EDirection.RIGHT && transform.rotation.y != 0f)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        float distance = Vector2.Distance(transform.position, m_player.transform.position);
        if(!m_characterController.IsAttacking)
        {
            if (distance > m_safeDistance)
            {
                Vector2 direction = m_player.transform.position - transform.position;
                m_characterController.Direction = direction;
                color = Color.red;
            }
            else
            {
                Attack();
                color = Color.green;
            }
        }
        else
        {
            float direction = m_characterController.FacingDirection == EDirection.LEFT ? 1f : -1f;
            Vector2 attackOrigin = new Vector2(transform.position.x + (direction * (m_sr.size.x / 2f)),
                                                transform.position.y + m_sr.size.y / 2f);
            VisualDebug.DrawCross(attackOrigin, 0.15f, Color.green);
            Vector2 boxOrigin = new Vector2(attackOrigin.x + (direction * (m_attackX / 2f)), attackOrigin.y);
            VisualDebug.DrawBox(boxOrigin, new Vector2(m_attackX, m_attackY), Color.cyan);
        }

        Debug.DrawLine(transform.position, m_player.transform.position, color);
    }

    void Attack()
    {
        if (m_characterController.CanAttack)
        {
            m_characterController.LastAttack = Time.time;

            float direction = m_characterController.FacingDirection == EDirection.LEFT ? -1f : 1f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(transform.DORotate(new Vector3(0f, 0f, direction * -45f), 0.5f));
            sequence.Append(transform.DORotate(new Vector3(0f, 0f, direction *  65f), 0.1f));
            sequence.AppendCallback(() => { m_characterController.IsAttackActive = true; });
            sequence.AppendCallback(CheckAttackCollisions);
            sequence.AppendInterval(0.2f);
            sequence.AppendCallback(() => { m_characterController.IsAttackActive = false; });
            sequence.Append(transform.DORotate(new Vector3(0f, 0f, 0f), 0.25f));
            sequence.Play();
        }
    }

    private void CheckAttackCollisions()
    {
        float direction = m_characterController.FacingDirection == EDirection.LEFT ? 1f : -1f;
        Vector2 attackOrigin = new Vector2(transform.position.x + (direction * (m_sr.size.x / 2f)),
                                           transform.position.y + m_sr.size.y / 2f);

        Vector2 boxOrigin = new Vector2(attackOrigin.x + (direction * (m_attackX / 2f)), attackOrigin.y);
        Collider2D[] collisions = Physics2D.OverlapBoxAll(boxOrigin, new Vector2(m_attackX, m_attackY), 0f, LayerMask.GetMask("HitBox"));

        for (int i = 0; i < collisions.Length; i++)
        {
            if(collisions[i].CompareTag("Player"))
            {
                HealthBehaviour hittable = collisions[i].GetComponentInParent<HealthBehaviour>();
                if (hittable != null && hittable.gameObject != gameObject)
                {
                    hittable.Hit(gameObject, 10);
                }
            }
        }
    }
}
