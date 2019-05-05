using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform m_followTransform;
    [SerializeField] private float m_minX = 0f;

    private float m_x = 0f;

    void FixedUpdate()
    {

        if (m_followTransform.position.x > m_minX)
            m_x = m_followTransform.position.x;
        else
            m_x = m_minX;

        transform.position = new Vector3(m_x, transform.position.y, transform.position.z);
    }
}
