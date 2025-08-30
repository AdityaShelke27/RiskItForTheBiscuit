using System.Collections;
using UnityEditor;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] float m_WaveDelay;
    [SerializeField] Wave[] m_Waves;
    [SerializeField] GameObject[] m_Enemies;
    [SerializeField] Transform[] m_SpawnPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(StartWave());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator StartWave()
    {
        yield return new WaitForSeconds(2);

        for (int i = 0; i < 1/*m_Waves.Length*/; i++)
        {
            Wave wave = m_Waves[i];
            int enemyCount = wave.enemies.Length;
            for (int j = 0; j < enemyCount; j++)
            {
                GameObject enemy = Instantiate(m_Enemies[(int) wave.enemies[j].enemyType], m_SpawnPoints[wave.enemies[j].spawnIdx].position, Quaternion.identity);
                enemy.GetComponent<Enemy>().OnSpawned();

                yield return new WaitForSeconds(wave.timeInterval);
            }
            
            yield return new WaitForSeconds(m_WaveDelay);
        }
    }
}
