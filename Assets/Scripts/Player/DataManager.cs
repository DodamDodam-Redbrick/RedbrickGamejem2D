using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public enum EntityType
{
    //아군 엔티티
    tank = 0,
    sword = 1,
    bow = 2,
    caster = 3,
    healer = 4,

    //적 엔티티
    slime = 101,
    wolf = 102,
}

public enum RewardType
{
    gold,
    unit,
}

public enum ImageIndex
{
    map_boss,
    map_battle,
    map_shop,
    map_randomEvent,
    map_start,
}

public enum MapType
{
    stage_one,
    stage_two,
}

public class Reward
{
    public string imagePath;
    public string description;
    public RewardType rewardType;

    public Unit unit;
    public int gold;
}

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [SerializeField]
    Sprite bossSprite;

    [SerializeField]
    Sprite battleSprite;

    [SerializeField]
    Sprite shopSprite;

    [SerializeField]
    Sprite randomEventSprite;

    [SerializeField]
    Sprite startSprite;

    public static Dictionary<EntityType, Entity> EntityData = new Dictionary<EntityType, Entity>();
    public static Dictionary<RewardType, Reward> RewardData = new Dictionary<RewardType, Reward>();
    public static Dictionary<ImageIndex, Sprite> ImageData = new Dictionary<ImageIndex, Sprite>();

    public static Dictionary<MapType, List<SpawnData>> enemySpawners = new Dictionary<MapType, List<SpawnData>>();

    private void Awake()
    {
        DontDestroyOnLoad(this);

        Instance = this;

        ImageData[ImageIndex.map_boss] = bossSprite;
        ImageData[ImageIndex.map_battle] = battleSprite;
        ImageData[ImageIndex.map_randomEvent] = randomEventSprite;
        ImageData[ImageIndex.map_shop] = shopSprite;
        ImageData[ImageIndex.map_start] = startSprite;

        ApplyEnemySpawners();
    }

    private void ApplyEnemySpawners()
    {
        enemySpawners[MapType.stage_one] = CreateEnemySpawnerForStageOne();
        enemySpawners[MapType.stage_two] = CreateEnemySpawnerForStageTwo();
    }

    private List<SpawnData> CreateEnemySpawnerForStageOne() // stage one setting enemy
    {
        List<SpawnData> spawnData = new List<SpawnData>
        {
            new SpawnData { enemyInfo = EnemyInfo.Enemy1, spawnTime = 1.0f, wayPoints = new List<Vector3> { new Vector3(0, 0, 0), new Vector3(1, 1, 0) } },
            new SpawnData { enemyInfo = EnemyInfo.Enemy1, spawnTime = 2.0f, wayPoints = new List<Vector3> { new Vector3(2, 2, 0), new Vector3(3, 3, 0) } },
        };

        return spawnData;
    }

    private List<SpawnData> CreateEnemySpawnerForStageTwo() // stage two setting enemy
    {

        List<SpawnData> spawnData = new List<SpawnData>
        {
            new SpawnData { enemyInfo = EnemyInfo.Enemy2, spawnTime = 1.0f, wayPoints = new List<Vector3> { new Vector3(0, 0, 0), new Vector3(1, 1, 0) } },
            new SpawnData { enemyInfo = EnemyInfo.Enemy2, spawnTime = 2.0f, wayPoints = new List<Vector3> { new Vector3(2, 2, 0), new Vector3(3, 3, 0) } },
        };

        return spawnData;
    }
}