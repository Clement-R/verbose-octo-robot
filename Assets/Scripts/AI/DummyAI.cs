using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DummyAI : MonoBehaviour
{
    [SerializeField] private float m_safeDistance = 0.25f;

    private GameObject m_player;
    private CharacterController m_characterController;

    void Start()
    {
        m_player = GameObject.FindGameObjectWithTag("Player");
        m_characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, m_player.transform.position);
        if(distance > m_safeDistance)
        {
            Vector2 direction = m_player.transform.position - transform.position;
            m_characterController.Direction = direction;
        }
    }
}
