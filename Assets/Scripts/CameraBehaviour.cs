using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform m_followTransform;
    [SerializeField] private float m_minX = 0f;
    [SerializeField] private float m_minY = 5f;
    [SerializeField] private float m_freeRoamWidth = 2f;
    [SerializeField] private float m_freeRoamHeight = 2f;
    [SerializeField] private float m_freeRoamXOffset = 0f;
    [SerializeField] private float m_freeRoamYOffset = 0f;

    private float m_x = 0f;
    private float m_y = 0f;

    private float m_zoneX;
    private float m_zoneY;

    void FixedUpdate()
    {
        m_zoneX = (m_freeRoamWidth / 2) + m_freeRoamXOffset;
        m_zoneY = (m_freeRoamHeight / 2) + m_freeRoamYOffset;


        if (m_followTransform.position.x - m_freeRoamXOffset + (m_freeRoamWidth / 2) > m_minX)
        {
            if (m_followTransform.position.x > (transform.position.x + m_zoneX)) {
                m_x = m_followTransform.position.x - m_zoneX;
            } else if (m_followTransform.position.x < (transform.position.x + m_freeRoamXOffset - (m_freeRoamWidth / 2))) {
                m_x = m_followTransform.position.x + ((m_freeRoamWidth / 2) - m_freeRoamXOffset );
            } else {

            }

            //m_x = m_followTransform.position.x;
        }
        else
        {
            m_x = m_minX;
        }

        if (m_followTransform.position.y - m_freeRoamYOffset - (m_freeRoamHeight / 2) < m_minY) { 
            if (m_followTransform.position.y > (transform.position.y + m_zoneY))
            {
                m_y = m_followTransform.position.y - m_zoneY;
            }
            else if (m_followTransform.position.y < (transform.position.y + m_freeRoamYOffset - (m_freeRoamHeight / 2)))
            {
                m_y = m_followTransform.position.y + ((m_freeRoamHeight / 2) - m_freeRoamYOffset);
            }
            else
            {

            }
        }
        else
        {
            m_y = m_minY;
        }

        transform.position = new Vector3(m_x, m_y, transform.position.z);
    }

    private void OnDrawGizmosSelected()
    {

        var size = new Vector3(m_freeRoamWidth, m_freeRoamHeight, .5f);
        var pos = new Vector3(m_freeRoamXOffset, m_freeRoamYOffset, 0);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + pos, size);
    }
}
