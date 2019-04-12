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

    private Rigidbody2D m_rb2d;
    private Vector2 m_movement = new Vector2();

    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        m_movement = Vector2.zero;

        if (Input.GetKey(m_upKey))
            m_movement += new Vector2(0f, 1f);
        else if (Input.GetKey(m_downKey))
            m_movement += new Vector2(0f, -1f);

        if (Input.GetKey(m_leftKey))
            m_movement += new Vector2(-1f, 0f);
        else if (Input.GetKey(m_rightKey))
            m_movement += new Vector2(1f, 0f);
    }

    private void FixedUpdate()
    {
        Move(m_movement);
    }

    private void Move(Vector2 p_movement)
    {
        m_rb2d.velocity = m_movement * m_speed;
    }
}
