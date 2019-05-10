using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;

public class FloatingText : MonoBehaviour
{
    private TextMeshPro m_text;

    private void Awake()
    {
        m_text = GetComponent<TextMeshPro>();

        m_text.faceColor = new Color32(m_text.faceColor.r, m_text.faceColor.g, m_text.faceColor.b, (byte)0f);
        m_text.outlineColor = new Color32(m_text.outlineColor.r, m_text.outlineColor.g, m_text.outlineColor.b, (byte)0f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
            Init("66");
    }

    public void Init(string p_text)
    {
        m_text.text = p_text;
        StartCoroutine(_DoEffect());
    }

    private IEnumerator _DoEffect()
    {
        float t = 0f;

        float startY = transform.position.y;
        float endY = transform.position.y + .5f;
        float duration = 0.25f;

        while (t < duration)
        {
            float y = Mathf.Lerp(startY, endY, t / duration);
            transform.position = new Vector3(transform.position.x,
                                             y,
                                             transform.position.z);
            float a = Mathf.Lerp(0f, 255f, t / duration);
            m_text.faceColor = new Color32(m_text.faceColor.r, m_text.faceColor.g, m_text.faceColor.b, (byte)a);
            m_text.outlineColor = new Color32(m_text.outlineColor.r, m_text.outlineColor.g, m_text.outlineColor.b, (byte)a);

            yield return null;
            t += Time.deltaTime;
        }

        yield return new WaitForSeconds(0.25f);

        // Reset
        m_text.faceColor = new Color32(m_text.faceColor.r, m_text.faceColor.g, m_text.faceColor.b, (byte)0f);
        m_text.outlineColor = new Color32(m_text.outlineColor.r, m_text.outlineColor.g, m_text.outlineColor.b, (byte)0f);

        transform.position = new Vector3(transform.position.x,
                                         startY,
                                         transform.position.z);

        yield return null;
        Destroy(gameObject);
    }
}
