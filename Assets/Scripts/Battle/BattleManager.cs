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
    List<SpawnData> spawnDatas; //���� �������� ������ �� ���� �� ī��Ʈ �ؼ� ���� üũ

    [SerializeField]
    List<EnemySpawner> spawnPoints;

    [SerializeField]
    Transform mainCharacterPos;

    Unit mainCharacter;

    [SerializeField]
    int killCount = 0;

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

        mainCharacter = Instantiate(GameSystem.Instance.mainCharacter.entityPrefab, transform).GetComponentInChildren<Unit>();
        mainCharacter.transform.parent.position = mainCharacterPos.position;
        mainCharacter.transform.parent.rotation = mainCharacterPos.rotation;
        mainCharacter.Init(GameSystem.Instance.mainCharacter);
    }

    public void SpawnEnemies()
    {
        spawnDatas.Sort((x, y) => x.spawnTime.CompareTo(y.spawnTime)); // ���� �ð��� �������� ����

        StartCoroutine(CoSpawnEnemies());
    }

    public void CountKillCount()
    {
        killCount++;
        if (killCount >= spawnDatas.Count)
        {
            //��
            GameSystem.Instance.FinishBattle();
        }
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
                if (mainCharacter != null) //������ null��
                {
                    spawnDatas[spawnIndex].wayPoints.Add(mainCharacter.transform.position); //������ �������� ���� ĳ���ͷ�
                    spawnPoints[spawnDatas[spawnIndex].spawnerIndex].SpawnEnemy(spawnDatas[spawnIndex]);
                    spawnIndex++;
                }
            }

            yield return null;
        }

    }
}
