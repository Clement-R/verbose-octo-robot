using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeringBeahvior : MonoBehaviour
{
    private static int m_updateFrequency = 2;

    [SerializeField] private bool m_followParent = false;
    [Header("Don't specify parent if it's not needed")]
    [SerializeField] private GameObject m_parent = null;
    [SerializeField] private int m_followDelta = 0;

    private SpriteRenderer m_sr;

    private void Awake()
    {
        m_sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(!m_followParent)
            m_sr.sortingOrder = ((int) (transform.position.y * 1000f)) * -1;
        else
        {
            if(m_parent == null)
                m_sr.sortingOrder = (((int)(transform.parent.position.y * 1000f) - m_followDelta) * -1);
            else
                m_sr.sortingOrder = (((int)(m_parent.transform.position.y * 1000f) - m_followDelta) * -1);
        }
    }
}
