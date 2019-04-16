﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum EDirection
    {
        LEFT = 0,
        RIGHT = 1
    }

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

    private Rigidbody2D m_rb2d;
    private SpriteRenderer m_sr;
    private Vector2 m_movement = new Vector2();
    private EDirection m_facingDirection = EDirection.RIGHT;

    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
        m_sr = GetComponent<SpriteRenderer>();
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
        }

        if (m_facingDirection == EDirection.LEFT && !m_sr.flipX)
            m_sr.flipX = true;
        else if(m_facingDirection == EDirection.RIGHT && m_sr.flipX)
            m_sr.flipX = false;

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
    }

    private void Move(Vector2 p_movement)
    {
        m_rb2d.velocity = m_movement * m_speed;
    }

    private void Dash()
    {
        m_rb2d.AddForce(m_dashForce * (m_facingDirection == EDirection.RIGHT ? 1f : -1f), ForceMode2D.Force);
    }
}
