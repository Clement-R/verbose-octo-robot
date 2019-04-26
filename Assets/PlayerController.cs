﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EDirection
{
    LEFT = 0,
    RIGHT = 1
}

public class PlayerController : MonoBehaviour
{

    [SerializeField] private KeyCode m_upKey;
    [SerializeField] private KeyCode m_downKey;
    [SerializeField] private KeyCode m_leftKey;
    [SerializeField] private KeyCode m_rightKey;

    [SerializeField] private float m_speed = 2f;
    [SerializeField] private bool m_debug = false;

    [Header("Dash")]
    [SerializeField] private Vector2 m_dashForce = new Vector2();
    [SerializeField] private float m_dashCooldown = 0f;
    [SerializeField] private float m_dashDuration = 0.25f;
    [SerializeField] private KeyCode m_dashKey;
    private bool m_isDashing;
    private float m_lastDash = 0f;

    [Header("Jump")]
    [SerializeField] private KeyCode m_jumpKey;
    [SerializeField] float m_jumpHeight = 0f;
    [SerializeField] float m_jumpLength = 0f;

    [Header("Combat")]
    [SerializeField] private KeyCode m_attackKey;
    [SerializeField] private GameObject m_weapon;
    [SerializeField] private bool m_isAttackActive = false;
    [SerializeField] private float m_attackCooldown = 0.5f;
    [SerializeField] private float m_attackX = 1f;
    [SerializeField] private float m_attackY = 0.25f;
    [SerializeField] private TrailRenderer m_trail = null;

    [Header("Combat - Sword Parameters")]
    [SerializeField] private float m_attackAnticipation = 0.1f;
    [SerializeField] private float m_attackStrike = 0.1f;
    [SerializeField] private float m_attackWait = 0.1f;
    [SerializeField] private float m_attackRecovery = 0.05f;

    private bool m_isAttacking { get { return Time.time < m_lastAttack + m_attackCooldown; } }
    private float m_lastAttack;

    private float m_initialVelocity = 0f;
    private float m_gravity = 0f;
    private bool m_grounded = true;
    private bool m_jump = false;

    private Rigidbody2D m_rb2d;
    private SpriteRenderer m_sr;
    private Vector2 m_movement = new Vector2();
    public EDirection m_facingDirection = EDirection.RIGHT;

    private Animator m_animator;

    private bool m_isJumping = false;
    private float m_lastJump = 0f;

    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_sr = GetComponentInChildren<SpriteRenderer>();
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

    private void FixedUpdate()
    {
        Move(m_movement);

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
            m_animator.SetBool("jump", false);
        }

        if(m_isJumping)
        {
            if(Time.time >= m_lastJump + m_jumpLength)
            {
                m_isJumping = false;
                m_rb2d.gravityScale = 0f;
                m_rb2d.velocity = new Vector2(m_rb2d.velocity.x, 0f);
            }
        }
    }

    private void Move(Vector2 p_movement)
    {
        m_rb2d.velocity = m_movement * m_speed;
    }

    private void Dash()
    {
        m_rb2d.AddForce(m_dashForce * (m_facingDirection == EDirection.RIGHT ? 1f : -1f), ForceMode2D.Force);
    }

    private void Jump()
    {
        m_initialVelocity = 2f * m_jumpHeight / (m_jumpLength / 2f);
        m_gravity = (-2f * m_jumpHeight) / ((m_jumpLength / 2f) * (m_jumpLength / 2f));
        m_rb2d.gravityScale = m_gravity / Physics2D.gravity.y;

        m_rb2d.AddForce(new Vector2(0f, m_initialVelocity), ForceMode2D.Impulse);
    }

    private void Attack()
    {
        if(Time.time > m_lastAttack + m_attackCooldown)
        {
            m_lastAttack = Time.time;

            float direction = m_facingDirection == EDirection.LEFT ? -1f : 1f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(m_weapon.transform.DORotate(new Vector3(0f, 0f, direction * 25f), m_attackAnticipation));
            sequence.AppendCallback(() => { m_trail.gameObject.SetActive(true); });
            sequence.Append(m_weapon.transform.DORotate(new Vector3(0f, 0f, direction * -90f), m_attackStrike));
            sequence.AppendCallback(() => { m_isAttackActive = true; });
            sequence.AppendCallback(CheckAttackCollisions);
            sequence.AppendInterval(m_attackWait);
            sequence.AppendCallback(() => { m_isAttackActive = false; });
            sequence.AppendCallback(() => { m_trail.gameObject.SetActive(false); });
            sequence.Append(m_weapon.transform.DORotate(new Vector3(0f, 0f, 0f), m_attackRecovery));
            sequence.Play();
        }
    }

    private void CheckAttackCollisions()
    {
        float direction = m_facingDirection == EDirection.LEFT ? -1f : 1f;
        Vector2 attackOrigin = new Vector2(transform.position.x + (direction * (m_sr.size.x / 2f)),
                                           transform.position.y + m_sr.size.y / 2f);

        Vector2 boxOrigin = new Vector2(attackOrigin.x + (direction * (m_attackX / 2f)), attackOrigin.y);
        Collider2D[] collisions = Physics2D.OverlapBoxAll(boxOrigin, new Vector2(m_attackX, m_attackY), 0f);

        for (int i = 0; i < collisions.Length; i++)
        {
            EnemyBehavior enemy = collisions[i].GetComponent<EnemyBehavior>();
            if (enemy != null)
                enemy.Attack(gameObject, 10);
        }
    }
}
