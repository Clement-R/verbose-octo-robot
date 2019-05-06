using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaInstance : MonoBehaviour
{
    public Action OnEnterArena;

    [SerializeField] private ColliderEvents m_gateTrigger;
    [SerializeField] private GameObject m_walls;

    private WaveManager m_waveManager;
    private bool m_isBattleStarted = false;

    private void Start()
    {
        m_waveManager = GetComponent<WaveManager>();
        m_waveManager.OnBattleEnd += EndBattle;
        m_gateTrigger.OnExitTrigger += StartBattle;
        m_walls.SetActive(false);
    }

    private void StartBattle(Collider2D p_collider)
    {
        Debug.Log("Start battle !");

        if(p_collider.gameObject.CompareTag("Player") && !m_isBattleStarted)
        {
            m_isBattleStarted = true;
            m_walls.SetActive(true);
        }

        m_waveManager.StartBattle();

        OnEnterArena?.Invoke();
    }

    private void EndBattle()
    {
        Debug.Log("Battle done !");
        m_walls.SetActive(false);
    }
}
