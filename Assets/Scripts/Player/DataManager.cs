using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.UIElements;

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

    //그 외
    gold = 300,
}
public enum UnitType
{
    sword = EntityType.sword,
    //밑에는 값 만들고 주석 풀기
    //tank = EntityType.tank,
    //bow = EntityType.bow,
    //healer = EntityType.healer,
    //caster = EntityType.caster,
}

public enum RewardType
{
    gold = EntityType.gold,
    unit_sword = EntityType.sword,
    //마찬가지
    //unit_tank = EntityType.tank,
    //unit_bow = EntityType.bow,
    //unit_healer = EntityType.healer,
    //unit_caster = EntityType.caster,
}

public enum ImageIndex
{
    unit_sword_thumbnail = EntityType.sword,

    unit_enemySlime_thumbnail = EntityType.slime,
    unit_enemyWolf_thumbnail = EntityType.wolf,

    reward_gold = EntityType.gold,

    map_boss,
    map_battle,
    map_shop,
    map_randomEvent,
    map_start,
    map_unknown,
}

public enum MapType
{
    firstStage_one,
    firstStage_two,
    firstStage_three,
}

public enum EventType
{
    one,
    two,
    three,

}

public class Reward
{
    public Reward(Sprite thumbnail, string description, RewardType rewardType)
    {
        SetInfo(thumbnail, description, rewardType);
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

public class Event
{
    public Event(string mainEvent, List<string> options, List<RewardType> rewardTypes, EventType eventType)
    {
        SetEventInfo(mainEvent, options, eventType);
    }

    void SetEventInfo(string mainEvent, List<string> options, EventType eventType)
    {
        this.mainEvent = mainEvent;
        this.options = options;
        this.eventType = eventType;
    }

    public string mainEvent;
    public EventType eventType;

    public List<string> options;
    public List<RewardType> rewardTypes;
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
    Sprite unknownSprite;

    [SerializeField]
    Sprite goldSprite;

    [SerializeField]
    Sprite swordThumbnail;

    [SerializeField]
    GameObject swordPrefab;

    [Header("Enemy Info")]
    [SerializeField] 
    GameObject slimePrefab;
    [SerializeField]
    Sprite slimeSprite;

    [SerializeField] 
    GameObject wolfPrefab;
    [SerializeField]
    Sprite wolfSprite;

    public static Dictionary<ImageIndex, Sprite> imageData = new Dictionary<ImageIndex, Sprite>(); //이미지 데이터 관리용
    public static Dictionary<EntityType, GameObject> prefabData = new Dictionary<EntityType, GameObject>(); //프리팹 데이터 관리용
    public static Dictionary<MapType, List<SpawnData>> enemySpawners = new Dictionary<MapType, List<SpawnData>>(); //맵 별 스폰데이터 관리용

    public static Dictionary<RewardType, Reward> rewardData = new Dictionary<RewardType, Reward>(); //보상 데이터 저장용
    public static Dictionary<EntityType, Entity> entityData = new Dictionary<EntityType, Entity>(); //엔티티 데이터 저장용
    public static Dictionary<EventType, Event> eventData = new Dictionary<EventType, Event>(); // 이벤트 데이터 저장용
  

    private void Awake()
    {
        DontDestroyOnLoad(this);

        Instance = this;

        ApplyImageDatas(); //항상 이미지가 가장 먼저
        ApplyPrefabDatas();
        ApplyEnemySpawners();
        ApplyRewardDatas();
        ApplyEntityDatas();
        ApplyEventDatas();
    }

    private void ApplyImageDatas()
    {
        imageData[ImageIndex.map_boss] = bossSprite;
        imageData[ImageIndex.map_battle] = battleSprite;
        imageData[ImageIndex.map_randomEvent] = randomEventSprite;
        imageData[ImageIndex.map_shop] = shopSprite;
        imageData[ImageIndex.map_start] = startSprite;
        imageData[ImageIndex.map_unknown] = unknownSprite;

        imageData[ImageIndex.reward_gold] = goldSprite;

        imageData[ImageIndex.unit_sword_thumbnail] = swordThumbnail;

        // Enemy Image
        imageData[ImageIndex.unit_enemySlime_thumbnail] = slimeSprite;
        imageData[ImageIndex.unit_enemyWolf_thumbnail] = wolfSprite;
    }

    private void ApplyRewardDatas()
    {
        //일반 보상 넣는 법
        rewardData[RewardType.gold] = new Reward(imageData[ImageIndex.reward_gold], "돈 입니다.", RewardType.gold);

        //유닛 보상 넣는 법 | 스탯 순서(hp, damagae, def, moveSpeed, fireRate, skillCoolTime, weight)
        EntityStats unitStat = new EntityStats(100, 5, 1, 1, 1, 10, 1);
        //원거리 유닛이면 총알 맨 뒤에 추가해줘야함
        UnitInfo unit = new UnitInfo(unitStat, UnitType.sword, imageData[ImageIndex.unit_sword_thumbnail], prefabData[EntityType.sword]);
        rewardData[RewardType.unit_sword] = new Reward(unit.thumbnail, "근거리에서 싸우는 유닛입니다", RewardType.unit_sword);
    }

    private void ApplyPrefabDatas()
    {
        prefabData[EntityType.sword] = swordPrefab;

        prefabData[EntityType.slime] = slimePrefab;
        prefabData[EntityType.wolf] = wolfPrefab;

    }

    private void ApplyEnemySpawners()
    {
        enemySpawners[MapType.firstStage_one] = new List<SpawnData>
        {
            new SpawnData { enemyType = EntityType.wolf, spawnTime = 1.0f, wayPoints = new List<Vector3> { new Vector3(0, 0, 0), new Vector3(1, 1, 0) } },
            new SpawnData { enemyType = EntityType.wolf, spawnTime = 2.0f, wayPoints = new List<Vector3> { new Vector3(12, 10, 0), new Vector3(3, 3, 0) } },
        };

        enemySpawners[MapType.firstStage_two] = new List<SpawnData>
        {
            new SpawnData { enemyType = EntityType.slime, spawnTime = 1.0f, wayPoints = new List<Vector3> { new Vector3(0, 0, 0), new Vector3(1, 1, 0) } },
            new SpawnData { enemyType = EntityType.wolf, spawnTime = 2.0f, wayPoints = new List<Vector3> { new Vector3(12, 10, 0), new Vector3(3, 3, 0) } },
        };

        enemySpawners[MapType.firstStage_three] = new List<SpawnData>
        {
            new SpawnData { enemyType = EntityType.slime, spawnTime = 1.0f, wayPoints = new List<Vector3> { new Vector3(0, 0, 0), new Vector3(1, 1, 0) } },
            new SpawnData { enemyType = EntityType.wolf, spawnTime = 2.0f, wayPoints = new List<Vector3> { new Vector3(12, 10, 0), new Vector3(3, 3, 0) } },
        };


    }

    private void ApplyEntityDatas()
    {
        EntityStats swordStat = new EntityStats(100, 5, 1, 1, 1, 10, 1);
        entityData[EntityType.sword] = new UnitInfo(swordStat, UnitType.sword, imageData[ImageIndex.unit_sword_thumbnail], swordPrefab);


        EntityStats slimeStat = new EntityStats(50, 3, 0, 1, 1, 10, 1);
        entityData[EntityType.slime] = new EnemyInfo(slimeStat, EntityType.slime, slimePrefab);

        EntityStats wolfStat = new EntityStats(50, 3, 0, 1, 1, 10, 1);
        entityData[EntityType.wolf] = new EnemyInfo(wolfStat, EntityType.wolf, wolfPrefab);

    }

    private void ApplyEventDatas()
    {
        // 옵션 별 보상 추가해야함. 

        //이벤트 별 텍스트 넣는 법 : 메인 이벤트 , 선택지들 , 이벤트타입
        eventData[EventType.one] = new Event("메인 이벤트_01", 
        new List<string>
        {
            "옵션 1",
            "옵션 2",
            "옵션 3",
        },

        new List<RewardType>
        { 
            RewardType.gold,
            RewardType.unit_sword,
            RewardType.gold,
        },

        EventType.one);


        eventData[EventType.two] = new Event("메인 이벤트_02",
        new List<string>
        {
            "옵션 1",
            "옵션 2",
            "옵션 3",
            "옵션 4",
        },


        new List<RewardType>
        {
            RewardType.gold,
            RewardType.unit_sword,
            RewardType.gold,
            RewardType.unit_sword,
        },

        EventType.two);


        eventData[EventType.three] = new Event("메인 이벤트_02",
        new List<string>
        {
            "옵션 1",
            "옵션 2",
            "옵션 3",
            "옵션 4",
        },


        new List<RewardType>
        {
            RewardType.gold,
            RewardType.unit_sword,
            RewardType.gold,
            RewardType.unit_sword,
        },

        EventType.two);
    }
}