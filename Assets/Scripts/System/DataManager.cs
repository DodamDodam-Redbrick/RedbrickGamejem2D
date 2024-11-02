using AYellowpaper.SerializedCollections;
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

[System.Serializable]
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

    public string description;
    public RewardType rewardType;

    [HideInInspector]
    public int gold;

    [HideInInspector]
    public Sprite thumbnail;

    [HideInInspector]
    public UnitInfo unit;
}

[System.Serializable]
public class EventOption
{
    public string option;
    public EventOptionType optionType;
    public int gold;
    public UnitInfo unit;
}

[System.Serializable]
public class Event
{
<<<<<<< HEAD:Assets/Scripts/System/DataManager.cs
    public Event(string mainEvent, EventType eventType, List<EventOption> optionType)
    {
        SetEventInfo(mainEvent, eventType, optionType);
    }

    void SetEventInfo(string mainEvent, EventType eventType, List<EventOption> optionType)
    {
        this.content = mainEvent;
        this.options = optionType;
=======
    public Event(string mainEvent, List<string> options, List<RewardType> rewardTypes, EventType eventType)
    {
        SetEventInfo(mainEvent, options, eventType);
    }

    void SetEventInfo(string mainEvent, List<string> options, EventType eventType)
    {
        this.mainEvent = mainEvent;
        this.options = options;
>>>>>>> parent of 84b5558 (Merge branch 'main' into BattleSetting):Assets/Scripts/Player/DataManager.cs
        this.eventType = eventType;
    }

    public string content;
    public EventType eventType;

<<<<<<< HEAD:Assets/Scripts/System/DataManager.cs
    public List<EventOption> options;
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
=======
    public List<string> options;
    public List<RewardType> rewardTypes;
}

>>>>>>> parent of 84b5558 (Merge branch 'main' into BattleSetting):Assets/Scripts/Player/DataManager.cs

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [HideInInspector]
    public bool isFinishLoad;

    public Dictionary<ImageIndex, Sprite> imageData = new SerializedDictionary<ImageIndex, Sprite>(); //이미지 데이터 관리용

    public Dictionary<EntityType, GameObject> prefabData = new Dictionary<EntityType, GameObject>(); //프리팹 데이터 관리용

    public Dictionary<MapType, GameObject> mapDatas = new Dictionary<MapType, GameObject>(); //맵 별 스폰데이터 관리용

    [SerializedDictionary("RewardType", "Reward")]
    public SerializedDictionary<RewardType, Reward> rewardData = new SerializedDictionary<RewardType, Reward>(); //보상 데이터 저장용

    [SerializedDictionary("UnitType", "UnitInfo")]
    public SerializedDictionary<UnitType, UnitInfo> unitData = new SerializedDictionary<UnitType, UnitInfo>(); //엔티티 데이터 저장용

    [SerializedDictionary("EnemyType", "EnemyInfo")]
    public SerializedDictionary<EnemyType, EnemyInfo> enemyData = new SerializedDictionary<EnemyType, EnemyInfo>(); //엔티티 데이터 저장용

    [SerializedDictionary("EventType", "Event")]
    public SerializedDictionary<EventType, Event> eventData = new SerializedDictionary<EventType, Event>(); // 이벤트 데이터 저장용

    private void Awake()
    {
        DontDestroyOnLoad(this);

        Instance = this;

        ApplyImageDatas(); //항상 이미지가 가장 먼저
        ApplyPrefabDatas();
        ApplyMapData();


        ApplyUnitDatas();
        ApplyRewardDatas();
        
        isFinishLoad = true;
    }

    private void ApplyUnitDatas()
    {
        foreach (UnitInfo item in unitData.Values)
        {
            item.thumbnail = imageData[(ImageIndex)item.unitType];
        }
    }

    private void ApplyRewardDatas()
    {
        foreach(Reward item in rewardData.Values)
        {
            item.thumbnail = imageData[(ImageIndex)item.rewardType];
            item.unit = unitData[(UnitType)item.rewardType];
        }
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

    private void ApplyPrefabDatas()
    {
        prefabData[EntityType.sword] = Resources.Load<GameObject>("Battle/Prefabs/Sword");

    }

    private void ApplyMapData()
    {
        mapDatas[MapType.firstStage_one] = Resources.Load<GameObject>("Battle/Prefabs/FirstMap");
    }

<<<<<<< HEAD:Assets/Scripts/System/DataManager.cs
=======
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
>>>>>>> parent of 84b5558 (Merge branch 'main' into BattleSetting):Assets/Scripts/Player/DataManager.cs
}