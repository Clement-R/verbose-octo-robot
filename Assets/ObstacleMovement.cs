using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class ObstacleMovement : MonoBehaviour
{
    public enum EMovementType
    {
        BETWEEN_TRANSFORM = 0
    }

    [SerializeField] private EMovementType m_movementType;

    [SerializeField] private Vector2 m_startPosition;
    [SerializeField] private Vector2 m_endPosition;
    [SerializeField] private float m_duration;

    private Rigidbody2D m_rb2d;
    private Vector3 m_target;

    void Start()
    {
        m_rb2d = GetComponent<Rigidbody2D>();

        Sequence mySequence = DOTween.Sequence();

        switch (m_movementType)
        {
            case EMovementType.BETWEEN_TRANSFORM:
                mySequence.Append(m_rb2d.DOMove(m_endPosition, m_duration));
                mySequence.Append(m_rb2d.DOMove(m_startPosition, m_duration));
                mySequence.SetLoops(-1);
                mySequence.Play();
                break;
        }
    }
}
