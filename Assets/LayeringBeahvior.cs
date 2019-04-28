using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayeringBeahvior : MonoBehaviour
{
    private static int m_updateFrequency = 2;

    [SerializeField] private bool m_followParent = false;
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
            m_sr.sortingOrder = (((int)(transform.parent.position.y * 1000f) - m_followDelta) * -1);
    }
}
