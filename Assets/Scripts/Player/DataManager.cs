using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public enum EntityType
{
    //¾Æ±º ¿£Æ¼Æ¼
    tank = 0,
    sword = 1,
    bow = 2,
    caster = 3,
    healer = 4,

    //Àû ¿£Æ¼Æ¼
    slime = 100,
    wolf = 101,

    //ÃÑ¾Ë ¿£Æ¼Æ¼
    bowBullet = 200,
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

    unit_sword_thumbnail,
}

public enum MapType
{
    stage_one,
    stage_two,
}

public class Reward
{
    public Reward(Sprite thumbnail, string description, RewardType rewardType, int gold = 0)
    {
        SetInfo(thumbnail, description, rewardType);
        this.gold = gold;
    }

    public Reward(Sprite thumbnail, string description, RewardType rewardType, UnitInfo unit = null)
    {
        SetInfo(thumbnail, description, rewardType);
        this.unit = unit;
    }

    void SetInfo(Sprite thumbnail, string description, RewardType rewardType)
    {
        this.thumbnail = thumbnail;
        this.description = description;
        this.rewardType = rewardType;
    }

    public Sprite thumbnail;
    public string description;
    public RewardType rewardType;

    public UnitInfo unit;
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

    [SerializeField]
    Sprite swordSprite;

    [SerializeField]
    GameObject swordPrefab;

    public static Dictionary<RewardType, Reward> rewardData = new Dictionary<RewardType, Reward>(); //º¸»ó¿¡ ÇÊ¿äÇÑ Á¤º¸µé
    public static Dictionary<ImageIndex, Sprite> imageData = new Dictionary<ImageIndex, Sprite>(); //°¢ ÀÌ¹ÌÁöµé °ü¸®ÇÏ´Â ¿ë
    public static Dictionary<EntityType, GameObject> prefabData = new Dictionary<EntityType, GameObject>(); //°¢ ÇÁ¸®ÆÕµé °ü¸®ÇÏ´Â ¿ë
    public static Dictionary<EntityType, Entity> entityData = new Dictionary<EntityType, Entity>(); //¼ÒÈ¯ÇÒ ¶§ ÇÁ¸®ÆÕ °ª ÃÊ±âÈ­ ¿ë

    public static Dictionary<MapType, List<SpawnData>> enemySpawners = new Dictionary<MapType, List<SpawnData>>();

    private void Awake()
    {
        DontDestroyOnLoad(this);

        Instance = this;

        imageData[ImageIndex.map_boss] = bossSprite;
        imageData[ImageIndex.map_battle] = battleSprite;
        imageData[ImageIndex.map_randomEvent] = randomEventSprite;
        imageData[ImageIndex.map_shop] = shopSprite;
        imageData[ImageIndex.map_start] = startSprite;
        imageData[ImageIndex.unit_sword_thumbnail] = swordSprite;

        prefabData[EntityType.sword] = swordPrefab;

        ApplyEnemySpawners();
        
        //Áß¿ä!! ÀÌ¹ÌÁö µ¥ÀÌÅÍ¶û ÇÁ¸®ÆÕ µ¥ÀÌÅÍº¸´Ù µÚ¿¡¿Ã°Í
        EntityStats swordStat = new EntityStats(100, 5, 1, 1, 1, 10, 1);
        entityData[EntityType.sword] = new UnitInfo(swordStat, UnitType.sword, imageData[ImageIndex.unit_sword_thumbnail], swordPrefab);

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