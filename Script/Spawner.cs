using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float timeBetweenSpawn = 5f;
    [SerializeField] private int maxActiveEnemies = 5;
    [SerializeField] private List<EnemyControl> enemyPrefabs = new List<EnemyControl>();
    [SerializeField] private List<float> spawnWeights = new List<float>();

    private IObjectPool<EnemyControl> enemyPool;
    private int activeEnemyCount = 0;
    private float timeSinceLastSpawn;
    private float totalSpawnWeight;

    private void Awake()
    {
        totalSpawnWeight = spawnWeights.Sum();

        while (spawnWeights.Count < enemyPrefabs.Count)
        {
            spawnWeights.Add(1f);
        }

        enemyPool = new ObjectPool<EnemyControl>(
            CreateEnemy,
            OnGetFromPool,
            OnReturnToPool,
            OnDestroyEnemy,
            maxSize: 20
        );
    }

    private EnemyControl CreateEnemy()
    {
        int randomIndex = GetRandomEnemyIndex();
        EnemyControl enemy = Instantiate(enemyPrefabs[randomIndex]);
        enemy.SetPool(enemyPool);
        enemy.gameObject.SetActive(false);
        return enemy;
    }

    private int GetRandomEnemyIndex()
    {
        float totalWeight = spawnWeights.Sum();
        float randomPoint = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            currentWeight += spawnWeights[i];
            if (randomPoint <= currentWeight)
                return i;
        }

        return 0;
    }

    private void OnGetFromPool(EnemyControl enemy)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        enemy.transform.position = spawnPoint.position;
        enemy.transform.rotation = spawnPoint.rotation;

        enemy.gameObject.SetActive(true);
        enemy.ResetEnemy();

        activeEnemyCount++;
    }

    private void OnReturnToPool(EnemyControl enemy)
    {
        enemy.gameObject.SetActive(false);
        activeEnemyCount--;
    }

    private void OnDestroyEnemy(EnemyControl enemy)
    {
        Destroy(enemy.gameObject);
    }

    private void Update()
    {
        if (activeEnemyCount < maxActiveEnemies && Time.time > timeSinceLastSpawn)
        {
            enemyPool.Get();
            timeSinceLastSpawn = Time.time + timeBetweenSpawn;
        }
    }
}