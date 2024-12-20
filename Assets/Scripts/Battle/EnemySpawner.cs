using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private List<SpawnData> spawnDataList;

    public void SpawnEnemy(SpawnData spawnData)
    {
        EnemyInfo enemyInfo = DataManager.Instance.enemyData[spawnData.enemyType];
        GameObject enemyPrefab = enemyInfo.entityPrefab;
        if (enemyPrefab != null)
        {
            Enemy enemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity).GetComponent<Enemy>();
            enemy.transform.SetParent(transform.parent, true);

            EnemyInfo instEnemyInfo = enemyInfo.DeepCopy();
            enemy.Init(instEnemyInfo);

            if (enemy != null)
            {
                enemy.SetMapGrid();
                enemy.SetWayPoints(spawnData.wayPoints);
                enemy.StartMove();
            }

        }
    }

}


