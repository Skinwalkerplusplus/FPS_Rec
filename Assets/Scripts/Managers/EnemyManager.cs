using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float spawnDelay = 1f;

    public int currentEnemyCount = -3;

    void Start()
    {
        GameManager.deadEnemy += EnemyKilled;

        SpawnEnemy();
    }

    public void EnemyKilled()
    {
        currentEnemyCount--;

        if (currentEnemyCount <= 5)
        {
            Invoke("SpawnEnemy", spawnDelay);
        }
    }

    void SpawnEnemy()
    {
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject enemyPrefab = enemyPrefabs[randomIndex];

        int randomSpawnPoint = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomSpawnPoint];

        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        currentEnemyCount++;
    }
}
