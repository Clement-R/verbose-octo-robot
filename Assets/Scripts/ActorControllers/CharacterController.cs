using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public Vector2 Direction = new Vector2();
    public float Speed { get { return m_speed;  } }

    [SerializeField] protected float m_speed = 2f;

    [Header("Dash")]
    [SerializeField] protected Vector2 m_dashForce = new Vector2();
    [SerializeField] protected float m_dashCooldown = 0f;
    [SerializeField] protected float m_dashDuration = 0.25f;
    protected bool m_isDashing;
    protected float m_lastDash = 0f;

    [Header("Jump")]
    [SerializeField] protected float m_jumpHeight = 0f;
    [SerializeField] protected float m_jumpLength = 0f;
    protected float m_initialVelocity = 0f;
    protected float m_gravity = 0f;
    protected bool m_grounded = true;
    protected bool m_jump = false;
    protected bool m_isJumping = false;
    protected float m_lastJump = 0f;

    [Header("Combat")]
    [SerializeField] public WeaponHolder m_weapon;
    [SerializeField] protected bool m_isAttackActive = false;
    [SerializeField] protected float m_attackCooldown = 0.5f;
    [SerializeField] protected float m_attackX = 1f;
    [SerializeField] protected float m_attackY = 0.25f;
    [SerializeField] protected TrailRenderer m_trail = null;

    public EDirection m_facingDirection = EDirection.RIGHT;

    protected Rigidbody2D m_rb2d;
    protected SpriteRenderer m_sr;

    protected bool m_isAttacking { get { return Time.time < m_lastAttack + m_attackCooldown; } }
    protected float m_lastAttack;

    protected virtual void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected void Move(Vector2 p_movement)
    {
        m_rb2d.velocity = Direction * m_speed;
    }

    protected void Dash()
    {
        m_rb2d.AddForce(m_dashForce * (m_facingDirection == EDirection.RIGHT ? 1f : -1f), ForceMode2D.Force);
    }

    protected void Jump()
    {
        m_initialVelocity = 2f * m_jumpHeight / (m_jumpLength / 2f);
        m_gravity = (-2f * m_jumpHeight) / ((m_jumpLength / 2f) * (m_jumpLength / 2f));
        m_rb2d.gravityScale = m_gravity / Physics2D.gravity.y;

        m_rb2d.AddForce(new Vector2(0f, m_initialVelocity), ForceMode2D.Impulse);
    }

    protected virtual void FixedUpdate()
    {
        Move(Direction);

        if (m_isDashing)
        {
            Dash();

            if (Time.time >= m_lastDash + m_dashDuration)
                m_isDashing = false;
        }

        if (m_jump)
        {
            m_jump = false;
            m_lastJump = Time.time;
            m_isJumping = true;
            Jump();
        }

        if (m_isJumping)
        {
            if (Time.time >= m_lastJump + m_jumpLength)
            {
                m_isJumping = false;
                m_rb2d.gravityScale = 0f;
                m_rb2d.velocity = new Vector2(m_rb2d.velocity.x, 0f);
            }
        }
    }
}
