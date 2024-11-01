using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public EntityType enemyType;
    public float spawnTime;
    public List<Vector3> wayPoints;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private GameObject wolfPrefab;

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
        GameObject enemyPrefab = DataManager.entityData[spawnData.enemyType].entityPrefab;
        if (enemyPrefab != null)
        {
            Enemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemy.transform.SetParent(transform.parent, true);

            if (enemy != null)
            {
                enemy.SetMapGrid();
                enemy.SetWayPoints(spawnData.wayPoints);
                enemy.StartMove();
            }

        }
    }

}


