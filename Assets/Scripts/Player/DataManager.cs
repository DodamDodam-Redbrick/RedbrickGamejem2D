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
    slime = 100,
    wolf = 101,

    //엔티티 투사체
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

    public static Dictionary<ImageIndex, Sprite> imageData = new Dictionary<ImageIndex, Sprite>(); //이미지 데이터 관리용
    public static Dictionary<EntityType, GameObject> prefabData = new Dictionary<EntityType, GameObject>(); //프리팹 데이터 관리용
    public static Dictionary<MapType, List<SpawnData>> enemySpawners = new Dictionary<MapType, List<SpawnData>>(); //맵 별 스폰데이터 관리용

    public static Dictionary<RewardType, Reward> rewardData = new Dictionary<RewardType, Reward>(); //보상 데이터 저장용
    public static Dictionary<EntityType, Entity> entityData = new Dictionary<EntityType, Entity>(); //엔티티 데이터 저장용


    private void Awake()
    {
        DontDestroyOnLoad(this);

        Instance = this;

        ApplyImageDatas();
        ApplyPrefabDatas();
        ApplyEnemySpawners();
        ApplyEntityDatas();
    }

    private void ApplyImageDatas()
    {
        imageData[ImageIndex.map_boss] = bossSprite;
        imageData[ImageIndex.map_battle] = battleSprite;
        imageData[ImageIndex.map_randomEvent] = randomEventSprite;
        imageData[ImageIndex.map_shop] = shopSprite;
        imageData[ImageIndex.map_start] = startSprite;
        imageData[ImageIndex.unit_sword_thumbnail] = swordSprite;
    }

    private void ApplyPrefabDatas()
    {
        prefabData[EntityType.sword] = swordPrefab;

    }

    private void ApplyEnemySpawners()
    {
        enemySpawners[MapType.stage_one] = new List<SpawnData>
        {
            new SpawnData { enemyType = EntityType.wolf, spawnTime = 1.0f, wayPoints = new List<Vector3> { new Vector3(0, 0, 0), new Vector3(1, 1, 0) } },
            new SpawnData { enemyType = EntityType.wolf, spawnTime = 2.0f, wayPoints = new List<Vector3> { new Vector3(2, 2, 0), new Vector3(3, 3, 0) } },
        };
        enemySpawners[MapType.stage_two] = new List<SpawnData>
        {
            new SpawnData { enemyType = EntityType.slime, spawnTime = 1.0f, wayPoints = new List<Vector3> { new Vector3(0, 0, 0), new Vector3(1, 1, 0) } },
            new SpawnData { enemyType = EntityType.wolf, spawnTime = 2.0f, wayPoints = new List<Vector3> { new Vector3(2, 2, 0), new Vector3(3, 3, 0) } },
        };
    }

    private void ApplyEntityDatas()
    {
        EntityStats swordStat = new EntityStats(100, 5, 1, 1, 1, 10, 1);
        entityData[EntityType.sword] = new UnitInfo(swordStat, UnitType.sword, imageData[ImageIndex.unit_sword_thumbnail], swordPrefab);
    }
}