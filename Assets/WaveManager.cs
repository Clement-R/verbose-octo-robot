using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public Action OnBattleEnd;

    [SerializeField] private List<Wave> m_waves;

    private List<GameObject> m_enemies;
    private int m_waveIndex = 0;

    public void StartBattle()
    {
        SendNextWave();
    }

    private void SendNextWave()
    {
        Debug.Log("Send next wave : " + m_waveIndex);

        if (m_waveIndex < m_waves.Count)
        {
            Wave wave = m_waves[m_waveIndex];
            m_enemies = new List<GameObject>();

            for (int i = 0; i < wave.NumberOfEnemies; i++)
            {
                Vector2 position = new Vector2(Random.Range(-8f, 8f), Random.Range(0f, 4f));
                m_enemies.Add(Instantiate(wave.Enemy, position, Quaternion.identity));
            }

            m_waveIndex++;
            StartCoroutine(_WaitForWaveEnd());
        }
        else
            EndBattle();
    }

    [SerializeField] private int COUNT;

    private IEnumerator _WaitForWaveEnd()
    {
        Debug.Log("Wait for wave end");
        while (m_enemies.Any(e => e != null))
        {
            COUNT = m_enemies.Count;
            yield return null;
        }
        SendNextWave();
    }

    private void EndBattle()
    {
        Debug.Log("No more waves to send");
        OnBattleEnd?.Invoke();
    }
}
