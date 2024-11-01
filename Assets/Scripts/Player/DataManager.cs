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
    //caster = EntityType.caster,
}

public enum EnemyType
{
    slime = EntityType.slime,
    wolf = EntityType.wolf,
}

public enum RewardType
{
    gold = EntityType.gold,
    unit_sword = EntityType.sword,
    //마찬가지
    //unit_tank = EntityType.tank,
    //unit_bow = EntityType.bow,
    //unit_caster = EntityType.caster,
}

public enum ImageIndex
{
    unit_sword = EntityType.sword,

    unit_enemySlime = EntityType.slime,
    unit_enemyWolf = EntityType.wolf,

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
}

public enum EventType
{
    one,
    two,
    three,
}

public enum EventOptionType // 이벤트 옵션별 보상?
{
    gold = EntityType.gold,
    sword = EntityType.sword,
}

public enum ShopType
{
    gold = EntityType.gold,
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
    public Event(string mainEvent, List<string> options, List<EventOptionType> optionType, EventType eventType)
    {
        SetEventInfo(mainEvent, options, optionType, eventType);
    }

    void SetEventInfo(string mainEvent, List<string> options, List<EventOptionType> optionType, EventType eventType)
    {
        this.mainEvent = mainEvent;
        this.options = options;
        this.optionType = optionType;
        this.eventType = eventType;
    }

    public string mainEvent;
    public EventType eventType;

    public List<string> options;
    public List<EventOptionType> optionType;

    public int gold;
}

public class Shop
{
    public Shop(Sprite thumbnail, string description, ShopType shopType)
    {
        this.thumbnail = thumbnail;
        this.description= description;
        this.shopType = shopType;
    }

    public Sprite thumbnail;
    public string description;
    public ShopType shopType;
}



public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [HideInInspector]
    public bool isFinishLoad;

    public static Dictionary<ImageIndex, Sprite> imageData = new Dictionary<ImageIndex, Sprite>(); //이미지 데이터 관리용
    public static Dictionary<EntityType, GameObject> prefabData = new Dictionary<EntityType, GameObject>(); //프리팹 데이터 관리용
    public static Dictionary<MapType, GameObject> mapDatas = new Dictionary<MapType, GameObject>(); //맵 별 스폰데이터 관리용

    public static Dictionary<RewardType, Reward> rewardData = new Dictionary<RewardType, Reward>(); //보상 데이터 저장용
    public static Dictionary<EntityType, Entity> entityData = new Dictionary<EntityType, Entity>(); //엔티티 데이터 저장용
    public static Dictionary<EventType, Event> eventData = new Dictionary<EventType, Event>(); // 이벤트 데이터 저장용

    private void Awake()
    {
        DontDestroyOnLoad(this);

        Instance = this;

        ApplyImageDatas(); //항상 이미지가 가장 먼저
        ApplyPrefabDatas();
        ApplyMapData();
        ApplyRewardDatas();
        ApplyEntityDatas();
        ApplyEventDatas();

        isFinishLoad = true;
    }

    private void ApplyImageDatas()
    {
        imageData[ImageIndex.map_boss] = Resources.Load<Sprite>("MapGenerate/Sprites/Boss");
        imageData[ImageIndex.map_battle] = Resources.Load<Sprite>("MapGenerate/Sprites/Battle");
        imageData[ImageIndex.map_randomEvent] = Resources.Load<Sprite>("MapGenerate/Sprites/RandomEvent");
        imageData[ImageIndex.map_shop] = Resources.Load<Sprite>("MapGenerate/Sprites/Shop");
        imageData[ImageIndex.map_start] = Resources.Load<Sprite>("MapGenerate/Sprites/Start");
        imageData[ImageIndex.map_unknown] = Resources.Load<Sprite>("MapGenerate/Sprites/Unknown");

        imageData[ImageIndex.reward_gold] = Resources.Load<Sprite>("Reward/Sprites/Gold");

        imageData[ImageIndex.unit_sword] = Resources.Load<Sprite>("Battle/Sprites/Sword");

        // Enemy Image
        imageData[ImageIndex.unit_enemySlime] = Resources.Load<Sprite>("Battle/Sprites/Slime");
        imageData[ImageIndex.unit_enemyWolf] = Resources.Load<Sprite>("Battle/Sprites/Wolf");
    }

    private void ApplyRewardDatas()
    {
        //일반 보상 넣는 법
        rewardData[RewardType.gold] = new Reward(imageData[ImageIndex.reward_gold], "돈 입니다.", RewardType.gold);

        //유닛 보상 넣는 법 | 스탯 순서(hp, damagae, def, moveSpeed, fireRate, skillCoolTime, weight)
        EntityStats unitStat = new EntityStats(100, 5, 1, 1, 1, 10, 1);
        //원거리 유닛이면 총알 맨 뒤에 추가해줘야함
        UnitInfo unit = new UnitInfo(unitStat, UnitType.sword, imageData[ImageIndex.unit_sword], prefabData[EntityType.sword]);
        rewardData[RewardType.unit_sword] = new Reward(unit.thumbnail, "근거리에서 싸우는 유닛입니다", RewardType.unit_sword);
    }

    private void ApplyPrefabDatas()
    {
        prefabData[EntityType.sword] = Resources.Load<GameObject>("Battle/Prefabs/Sword");

    }

    private void ApplyMapData()
    {
        mapDatas[MapType.firstStage_one] = Resources.Load<GameObject>("Battle/Prefabs/FirstMap");
    }

    private void ApplyEntityDatas()
    {
        EntityStats swordStat = new EntityStats(100, 5, 1, 1, 1, 10, 1);
        entityData[EntityType.sword] = new UnitInfo(swordStat, UnitType.sword, imageData[ImageIndex.unit_sword], Resources.Load<GameObject>("Battle/Prefabs/Sword"));

        EntityStats slimeStat = new EntityStats(50, 3, 0, 1, 1, 10, 1);
        entityData[EntityType.slime] = new EnemyInfo(slimeStat, EnemyType.slime, Resources.Load<GameObject>("Battle/Prefabs/Slime"));

        //EntityStats wolfStat = new EntityStats(50, 3, 0, 1, 1, 10, 1);
        //entityData[EntityType.wolf] = new EnemyInfo(wolfStat, EnemyType.wolf, wolfPrefab);
    }

    private void ApplyEventDatas()
    {
        // 옵션 별 보상 추가해야함. 

        //이벤트 별 텍스트 넣는 법 : 메인 이벤트 , 선택지들 , 선택지별 보상, 이벤트타입

        // Event_01
        eventData[EventType.one] = new Event("메인 이벤트_01", 
        new List<string>
        {
            "돈 지급",
            "옵션 2",
            "옵션 3",
        },
        new List<EventOptionType>
        { 
            EventOptionType.gold,
            EventOptionType.sword,
            EventOptionType.gold,
        },
        EventType.one);

        // Event_02
        eventData[EventType.two] = new Event("메인 이벤트_02",
        new List<string>
        {
            "옵션 1",
            "옵션 2",
            "옵션 3",
            "옵션 4",
        },    
        new List<EventOptionType>
        {
            EventOptionType.gold,
            EventOptionType.sword,
            EventOptionType.gold,
            EventOptionType.sword,
        },
        EventType.two);

        // Event_03
        eventData[EventType.three] = new Event("메인 이벤트_03",
        new List<string>
        {
            "옵션 1",
            "옵션 2",
            "옵션 3",
            "옵션 4",
        },
        new List<EventOptionType>
        {
            EventOptionType.gold,
            EventOptionType.sword,
            EventOptionType.gold,
            EventOptionType.sword,
        },
        EventType.three);

        // Event_04
    }
}