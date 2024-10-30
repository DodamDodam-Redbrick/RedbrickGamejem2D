using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyInfo
{
    Enemy1,
    Enemy2,
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<SpawnData> spawnDataList; // 스폰 데이터를 받을 리스트
    [SerializeField] private GameObject enemy1Prefab;
    [SerializeField] private GameObject enemy2Prefab;

    private float timer = 0f;
    private int spawnIndex = 0;

    void Update()
    {
        timer += Time.deltaTime;

        while (spawnIndex < spawnDataList.Count && spawnDataList[spawnIndex].spawnTime <= timer)
        {
            SpawnEnemy(spawnDataList[spawnIndex].enemyInfo);
            spawnIndex++;
        }
    }

    private void SpawnEnemy(EnemyInfo enemyInfo)
    {
        GameObject enemyPrefab = null;
        switch (enemyInfo)
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
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.StartMove(); // 적 이동 시작
            }
        }
    }
}

[System.Serializable]
public class SpawnData
{
    public EnemyInfo enemyInfo;
    public float spawnTime;
}