using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float spawnRate = 2f;

    public void SpawnWave(int amount)
    {
        StartCoroutine(ProcessWave(amount));
    }

    IEnumerator ProcessWave(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnRandomEnemy();
            yield return new WaitForSeconds(spawnRate);
        }
    }

    void SpawnRandomEnemy()
    {
        int randP = Random.Range(0, spawnPoints.Length);
        int randE = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[randE], spawnPoints[randP].position, spawnPoints[randP].rotation);
    }
}