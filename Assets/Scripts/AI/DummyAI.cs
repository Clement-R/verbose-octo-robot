using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(BasicCombat))]
public class DummyAI : MonoBehaviour
{
    [SerializeField] private float m_safeDistance = 0.25f;

    private GameObject m_player;
    private CharacterController m_characterController;
    private SpriteRenderer m_sr;
    private BasicCombat m_combat;

    [SerializeField] protected float m_attackX = 1f;
    [SerializeField] protected float m_attackY = 0.25f;

    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_characterController = GetComponent<CharacterController>();
        m_sr = GetComponent<SpriteRenderer>();
        m_combat = GetComponent<BasicCombat>();
        m_combat.TargetTag = "Player";
    }

    void Update()
    {
        Color color = Color.blue;

        if (m_player.transform.position.x > transform.position.x)
            m_characterController.FacingDirection = EDirection.RIGHT;
        else
            m_characterController.FacingDirection = EDirection.LEFT;

        if (m_characterController.FacingDirection == EDirection.LEFT && transform.rotation.y != 180f)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        else if (m_characterController.FacingDirection == EDirection.RIGHT && transform.rotation.y != 0f)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        float distance = Vector2.Distance(transform.position, m_player.transform.position);
        if(!m_combat.IsAttacking)
        {
            if (distance > m_safeDistance)
            {
                Vector2 direction = m_player.transform.position - transform.position;
                m_characterController.Direction = direction;
                color = Color.red;
            }
            else
            {
                m_combat.Attack();
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
}
