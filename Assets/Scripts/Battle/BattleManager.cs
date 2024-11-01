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
    public MapGrid mapGrid;

    [SerializeField]
    List<SpawnData> spawnDatas;

    [SerializeField]
    List<EnemySpawner> spawnPoints;

    [SerializeField]
    GameObject mainCharacter;

    private void OnEnable()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        spawnDatas.Sort((x, y) => x.spawnTime.CompareTo(y.spawnTime)); // ���� �ð��� �������� ����

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
                spawnDatas[spawnIndex].wayPoints.Add(mainCharacter.transform.position); //������ �������� ���� ĳ���ͷ�
                spawnPoints[spawnDatas[spawnIndex].spawnerIndex].SpawnEnemy(spawnDatas[spawnIndex]);
                spawnIndex++;
            }

            yield return null;
        }

    }
}
