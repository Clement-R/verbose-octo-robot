using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    [SerializeField] private float m_chunkSize = 5f;
    [SerializeField] private List<GameObject> m_chunksPrefab = new List<GameObject>();

    private List<GameObject> m_chunks = new List<GameObject>();
    private System.Random m_random;
    private float m_nextXPosition = 0f;

    private void Start()
    {
        m_random = new System.Random(481516);

        for (int i = 0; i < 5; i++)
        {
            // TODO: Support different sized chunks
            GameObject chunkPrefab = m_chunksPrefab[m_random.Next(0, m_chunksPrefab.Count - 1)];
            GameObject chunk = Instantiate(chunkPrefab, new Vector2(m_nextXPosition, 0f), Quaternion.identity);
            m_chunks.Add(chunk);
            m_nextXPosition += m_chunkSize;
        }

        // TODO: Add walls at the beginning and end, each room has its own colliders
    }
}
