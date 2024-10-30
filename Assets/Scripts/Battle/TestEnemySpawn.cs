using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemySpawn : MonoBehaviour
{
    public EnemySpawner enemySpawner;

    private void Start()
    {
        List<SpawnData> spawnDataList = new List<SpawnData>
        {
            new SpawnData { enemyInfo = EnemyInfo.Enemy1, spawnTime = 1.0f, wayPoints = new List<Vector3> { new Vector3(5, 5, 0), new Vector3(12, 10, 0) } },
            new SpawnData { enemyInfo = EnemyInfo.Enemy1, spawnTime = 3.0f, wayPoints = new List<Vector3> { new Vector3(0, 2, 0), new Vector3(2, 2, 0) } },
            new SpawnData { enemyInfo = EnemyInfo.Enemy2, spawnTime = 2.0f, wayPoints = new List<Vector3> { new Vector3(1, 0, 0), new Vector3(1, 1, 0) } },
            new SpawnData { enemyInfo = EnemyInfo.Enemy2, spawnTime = 3.0f, wayPoints = new List<Vector3> { new Vector3(2, 0, 0), new Vector3(2, 1, 0) } },
        };

        enemySpawner.SpawnEnemies(spawnDataList);
    }
}
