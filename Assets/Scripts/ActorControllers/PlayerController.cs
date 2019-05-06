using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EDirection
{
    LEFT = 0,
    RIGHT = 1
}

public class PlayerController : CharacterController
{
    [Header("Keys")]
    [SerializeField] private KeyCode m_upKey;
    [SerializeField] private KeyCode m_downKey;
    [SerializeField] private KeyCode m_leftKey;
    [SerializeField] private KeyCode m_rightKey;
    [SerializeField] protected KeyCode m_dashKey;
    [SerializeField] protected KeyCode m_jumpKey;
    [SerializeField] protected KeyCode m_attackKey;

    [Header("Debug")]
    [SerializeField] private bool m_debug = false;

    private Animator m_animator;

    protected override void Start()
    {
        base.Start();
        m_animator = GetComponent<Animator>();
    }

    void Update()
    {
        m_movement = Vector2.zero;

        if(!m_isDashing)
        {
            if (Input.GetKey(m_upKey))
                m_movement += new Vector2(0f, 1f);
            else if (Input.GetKey(m_downKey))
                m_movement += new Vector2(0f, -1f);

            if (Input.GetKey(m_leftKey))
            {
                m_facingDirection = EDirection.LEFT;
                m_movement += new Vector2(-1f, 0f);
            }
            else if (Input.GetKey(m_rightKey))
            {
                m_facingDirection = EDirection.RIGHT;
                m_movement += new Vector2(1f, 0f);
            }

            m_animator.SetFloat("hSpeed", Mathf.Abs(m_movement.x));
            m_animator.SetFloat("vSpeed", Mathf.Abs(m_movement.y));
        }

        if (Input.GetKeyDown(m_jumpKey))
        {
            m_jump = true;
            m_animator.SetBool("jump", true);
        }

        if (Input.GetKeyDown(m_attackKey))
            Attack();

        // Attack debug
        if(m_isAttacking)
        {
            float direction = m_facingDirection == EDirection.LEFT ? -1f : 1f;
            Vector2 attackOrigin = new Vector2(transform.position.x + (direction * (m_sr.size.x / 2f)),
                                               transform.position.y + m_sr.size.y / 2f);
            VisualDebug.DrawCross(attackOrigin, 0.15f, Color.green);
            Vector2 boxOrigin = new Vector2(attackOrigin.x + (direction * (m_attackX / 2f)), attackOrigin.y);
            VisualDebug.DrawBox(boxOrigin, new Vector2(m_attackX, m_attackY), Color.cyan);
        }

        if (m_facingDirection == EDirection.LEFT && transform.rotation.y != 180f)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        else if (m_facingDirection == EDirection.RIGHT && transform.rotation.y != 0f)
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

        if (Input.GetKey(m_dashKey) && !m_isDashing && (Time.time >= m_lastDash + m_dashCooldown))
        {
            m_lastDash = Time.time;
            m_isDashing = true;
        }

        if (m_debug)
        {
            Debug.DrawRay(transform.position, transform.right * 5f, Color.blue);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if(m_jump)
        {
            m_animator.SetBool("jump", false);
        }
    }

    //! Move this part to a weapon part or something ? Let it do it's own
    //! animations and collision things

    private void Attack()
    {
        if (m_weapon.weaponData != null)
        {
            if (Time.time > m_lastAttack + m_attackCooldown)
            {
                m_lastAttack = Time.time;

                float direction = m_facingDirection == EDirection.LEFT ? -1f : 1f;

                // TODO: Externalize this to a Weapon script ?
                Sequence sequence = DOTween.Sequence();
                sequence.Append(m_weapon.transform.DORotate(new Vector3(0f, 0f, direction * 25f), m_weapon.weaponData.attackAnticipation));
                sequence.AppendCallback(() => { m_trail.gameObject.SetActive(true); });
                sequence.Append(m_weapon.transform.DORotate(new Vector3(0f, 0f, direction * -90f), m_weapon.weaponData.attackStrike));
                sequence.AppendCallback(() => { m_isAttackActive = true; });
                sequence.AppendCallback(CheckAttackCollisions);
                sequence.AppendInterval(m_weapon.weaponData.attackWait);
                sequence.AppendCallback(() => { m_isAttackActive = false; });
                sequence.AppendCallback(() => { m_trail.gameObject.SetActive(false); });
                sequence.Append(m_weapon.transform.DORotate(new Vector3(0f, 0f, 0f), m_weapon.weaponData.attackRecovery));
                sequence.Play();
            }
        } else
        {
            Debug.Log("No weapon equipped. Cannot attack");
        }
    }

    private void CheckAttackCollisions()
    {
        float direction = m_facingDirection == EDirection.LEFT ? -1f : 1f;
        Vector2 attackOrigin = new Vector2(transform.position.x + (direction * (m_sr.size.x / 2f)),
                                           transform.position.y + m_sr.size.y / 2f);

        Vector2 boxOrigin = new Vector2(attackOrigin.x + (direction * (m_attackX / 2f)), attackOrigin.y);
        Collider2D[] collisions = Physics2D.OverlapBoxAll(boxOrigin, new Vector2(m_attackX, m_attackY), 0f, LayerMask.GetMask("HitBox"));

        for (int i = 0; i < collisions.Length; i++)
        {
            HealthBehaviour hittable = collisions[i].GetComponentInParent<HealthBehaviour>();
            if (hittable != null && hittable.gameObject != gameObject)
            {
                hittable.Hit(gameObject, m_weapon.weaponData.attackDamage);
            }
        }
    }
}
