using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public EnemyType enemyType;
    public float spawnTime;
    public List<Vector3> wayPoints;
    public int spawnerIndex = 0;
}

public class BattleManager : MonoBehaviour 
{

    [SerializeField]
    List<SpawnData> spawnDatas;

    [SerializeField]
    List<EnemySpawner> spawnPoints;

    [SerializeField]
    GameObject mainCharacter;

    [HideInInspector]
    public MapGrid mapGrid;

    [HideInInspector]
    public Camera cam;

    public void StartBattle()
    {
        SpawnEnemies();
    }

    public void Init()
    {
        cam = GetComponentInChildren<Camera>();
        mapGrid = GetComponentInChildren<MapGrid>();
    }

    public void SpawnEnemies()
    {
        spawnDatas.Sort((x, y) => x.spawnTime.CompareTo(y.spawnTime)); // 스폰 시간을 기준으로 정렬

        StartCoroutine(CoSpawnEnemies());
    }

    IEnumerator CoSpawnEnemies()
    {
        float timer = 0f;
        int spawnIndex = 0;

        while(spawnIndex < spawnDatas.Count)
        {
            timer += Time.deltaTime;

            if (spawnDatas[spawnIndex].spawnTime <= timer)
            {
                spawnDatas[spawnIndex].wayPoints.Add(mainCharacter.transform.position); //무조건 마지막엔 메인 캐릭터로
                spawnPoints[spawnDatas[spawnIndex].spawnerIndex].SpawnEnemy(spawnDatas[spawnIndex]);
                spawnIndex++;
            }

            yield return null;
        }

    }
}
