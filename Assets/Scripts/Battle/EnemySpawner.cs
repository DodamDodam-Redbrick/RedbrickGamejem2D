using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyInfo // 적 정보
{
    Enemy1,
    Enemy2,
}

[System.Serializable]
public class SpawnData
{
    public EnemyInfo enemyInfo;
    public float spawnTime;
    public List<Vector3> wayPoints;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy1Prefab;
    [SerializeField] private GameObject enemy2Prefab;

    private List<SpawnData> spawnDataList;
    private float timer = 0f;
    private int nextSpawnIndex = 0;

    void Update()
    {
        if (spawnDataList == null || nextSpawnIndex >= spawnDataList.Count)
            return;

        timer += Time.deltaTime;

        while (nextSpawnIndex < spawnDataList.Count && spawnDataList[nextSpawnIndex].spawnTime <= timer)
        {
            SpawnEnemy(spawnDataList[nextSpawnIndex]);
            nextSpawnIndex++;
        }
    }

    public void SpawnEnemies(List<SpawnData> spawnDataList)
    {
        this.spawnDataList = spawnDataList;
        this.spawnDataList.Sort((x, y) => x.spawnTime.CompareTo(y.spawnTime)); // 스폰 시간을 기준으로 정렬
        timer = 0f;
        nextSpawnIndex = 0;
    }

    private void SpawnEnemy(SpawnData spawnData)
    {
        GameObject enemyPrefab = null;
        switch (spawnData.enemyInfo)
        {
            case EnemyInfo.Enemy1:
                enemyPrefab = enemy1Prefab;
                break;
            case EnemyInfo.Enemy2:
                enemyPrefab = enemy2Prefab;
                break;
        }

        if (enemyPrefab != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemy.transform.SetParent(transform.parent, true);
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            if (enemyScript != null)
            {
                enemyScript.SetMapGrid();
                enemyScript.SetWayPoints(spawnData.wayPoints);
                enemyScript.StartMove();
            }

        }
    }
}


