using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private Rigidbody2D m_rb2d;
    private Vector2 m_movement = new Vector2();

    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        m_movement = Vector2.zero;

        if(m_isDashing)
        {
            if (Input.GetKey(m_upKey))
                m_movement += new Vector2(0f, 1f);
            else if (Input.GetKey(m_downKey))
                m_movement += new Vector2(0f, -1f);

            if (Input.GetKey(m_leftKey))
                m_movement += new Vector2(-1f, 0f);
            else if (Input.GetKey(m_rightKey))
                m_movement += new Vector2(1f, 0f);
        }

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
        m_rb2d.AddForce(m_dashForce, ForceMode2D.Force);
    }
}
