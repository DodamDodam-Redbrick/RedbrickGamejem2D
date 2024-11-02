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
    mainCharacter = 0,

    sword_1 = 10,
    sword_2 = 11,
    sword_3 = 12,

    archer_1 = 20,
    archer_2 = 21,
    archer_3 = 22,

    wizard_1 = 30,
    wizard_2 = 31,
    wizard_3 = 32,

    soldier_1 = 40,
    soldier_2 = 41,
    soldier_3 = 42,

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
    mainCharacter = EntityType.mainCharacter,

    sword_1 = EntityType.sword_1,
    sword_2 = EntityType.sword_2,
    sword_3 = EntityType.sword_3,
    //밑에는 값 만들고 주석 풀기
    //tank = EntityType.tank,
    archer_1 = EntityType.archer_1,
    archer_2 = EntityType.archer_2,
    archer_3 = EntityType.archer_3,
    //caster = EntityType.caster,
    wizard_1 = EntityType.wizard_1,
    wizard_2 = EntityType.wizard_2,
    wizard_3 = EntityType.wizard_3,

    soldier_1 = EntityType.soldier_1,
    soldier_2 = EntityType.soldier_2,
    soldier_3 = EntityType.soldier_3,
}


public enum EnemyType
{
    slime = EntityType.slime,
    wolf = EntityType.wolf,
}

//보상 목록
public enum RewardType
{
    reward_gold = EntityType.gold,
    //Unit
    unit_sword_1 = EntityType.sword_1,
    unit_sword_2 = EntityType.sword_2,
    unit_sword_3 = EntityType.sword_3,

    unit_archer_1 = EntityType.archer_1,
    unit_archer_2 = EntityType.archer_2,
    unit_archer_3 = EntityType.archer_3,

    unit_wizard_1 = EntityType.wizard_1,
    unit_wizard_2 = EntityType.wizard_2,
    unit_wizard_3 = EntityType.wizard_3,

    unit_soldier_1 = EntityType.soldier_1,
    unit_soldier_2 = EntityType.soldier_2,
    unit_soldier_3 = EntityType.soldier_3,

    //Shop
    shop_potion_one,
    shop_potion_two,
    shop_potion_three,

}

public enum EventOptionType
{
    gold = EntityType.gold,
    unit_sword_1 = EntityType.sword_1,
    unit_sword_2 = EntityType.sword_2,
    unit_sword_3 = EntityType.sword_3,

    unit_archer_1 = EntityType.archer_1,
    unit_archer_2 = EntityType.archer_2,
    unit_archer_3 = EntityType.archer_3,

    unit_wizard_1 = EntityType.wizard_1,
    unit_wizard_2 = EntityType.wizard_2,
    unit_wizard_3 = EntityType.wizard_3,

    unit_soldier_1 = EntityType.soldier_1,
    unit_soldier_2 = EntityType.soldier_2,
    unit_soldier_3 = EntityType.soldier_3,
}
public enum ImageIndex
{
    unit_mainCha = EntityType.mainCharacter,
    reward_gold = EntityType.gold,

    //Unit
    unit_sword_1 = EntityType.sword_1,
    unit_sword_2 = EntityType.sword_2,
    unit_sword_3 = EntityType.sword_3,

    unit_archer_1 = EntityType.archer_1,
    unit_archer_2 = EntityType.archer_2,
    unit_archer_3 = EntityType.archer_3,

    unit_wizard_1 = EntityType.wizard_1,
    unit_wizard_2 = EntityType.wizard_2,
    unit_wizard_3 = EntityType.wizard_3,

    unit_soldier_1 = EntityType.soldier_1,
    unit_soldier_2 = EntityType.soldier_2,
    unit_soldier_3 = EntityType.soldier_3,

    //Shop
    shop_potion_one,
    shop_potion_two,
    shop_potion_three,

    unit_enemySlime = EntityType.slime,
    unit_enemyWolf = EntityType.wolf,

   

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

    public Reward(Sprite thumbnail, string description, RewardType shopType, int shopPrice, UnitInfo unit = null) // 상점용 생성자
    {
        SetInfo(thumbnail, description, shopType);
        this.unit = unit;
        this.shopPrice = shopPrice;
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
    public int shopPrice;

    [HideInInspector]
    public UnitInfo unit;

    [HideInInspector]
    public int gold;

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
    public Event(string mainEvent, EventType eventType, List<EventOption> optionType)
    {
        SetEventInfo(mainEvent, eventType, optionType);
    }

    void SetEventInfo(string mainEvent, EventType eventType, List<EventOption> optionType)
    {
        this.content = mainEvent;
        this.options = optionType;

        this.eventType = eventType;
    }

    public string content;
    public EventType eventType;

    public List<EventOption> options;
}

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
        foreach (Reward item in rewardData.Values)
        {
            if (imageData.ContainsKey((ImageIndex)item.rewardType))
            {
                item.thumbnail = imageData[(ImageIndex)item.rewardType];
            }
            else
            {
                Debug.LogWarning($"Key '{item.rewardType}' not found in imageData.");
            }
            if (unitData.ContainsKey((UnitType)item.rewardType))
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

        imageData[ImageIndex.unit_mainCha] = Resources.Load<Sprite>("Battle/Sprites/MainCh_Idle");

        imageData[ImageIndex.unit_sword_1] = Resources.Load<Sprite>("Battle/Sprites/Sword_Idle");
        imageData[ImageIndex.unit_sword_2] = Resources.Load<Sprite>("Battle/Sprites/Sword_Idle");
        imageData[ImageIndex.unit_sword_3] = Resources.Load<Sprite>("Battle/Sprites/Sword_Idle");

        imageData[ImageIndex.unit_archer_1] = Resources.Load<Sprite>("Battle/Sprites/Archer_Idle");
        imageData[ImageIndex.unit_archer_2] = Resources.Load<Sprite>("Battle/Sprites/Archer_Idle");
        imageData[ImageIndex.unit_archer_3] = Resources.Load<Sprite>("Battle/Sprites/Archer_Idle");

        imageData[ImageIndex.unit_wizard_1] = Resources.Load<Sprite>("Battle/Sprites/Wizard");
        imageData[ImageIndex.unit_wizard_2] = Resources.Load<Sprite>("Battle/Sprites/Wizard");
        imageData[ImageIndex.unit_wizard_3] = Resources.Load<Sprite>("Battle/Sprites/Wizard");

        imageData[ImageIndex.unit_soldier_1] = Resources.Load<Sprite>("Battle/Sprites/Soldier_");
        imageData[ImageIndex.unit_soldier_2] = Resources.Load<Sprite>("Battle/Sprites/Soldier_");
        imageData[ImageIndex.unit_soldier_3] = Resources.Load<Sprite>("Battle/Sprites/Soldier_");

        // Enemy Image
        imageData[ImageIndex.unit_enemySlime] = Resources.Load<Sprite>("Battle/Sprites/Slime");
        imageData[ImageIndex.unit_enemyWolf] = Resources.Load<Sprite>("Battle/Sprites/Wolf");

        // Shop
        imageData[ImageIndex.shop_potion_one] = Resources.Load<Sprite>("Shop/Sprites/Potion_one");
        imageData[ImageIndex.shop_potion_two] = Resources.Load<Sprite>("Shop/Sprites/Potion_Two");
        imageData[ImageIndex.shop_potion_three] = Resources.Load<Sprite>("Shop/Sprites/Potion_three");
    }

    private void ApplyPrefabDatas()
    {

        prefabData[EntityType.mainCharacter] = Resources.Load<GameObject>("Battle/Prefabs/MainCha");

        prefabData[EntityType.sword_1] = Resources.Load<GameObject>("Battle/Prefabs/Sword");
        prefabData[EntityType.sword_2] = Resources.Load<GameObject>("Battle/Prefabs/Sword");
        prefabData[EntityType.sword_3] = Resources.Load<GameObject>("Battle/Prefabs/Sword");

        prefabData[EntityType.archer_1] = Resources.Load<GameObject>("Battle/Prefabs/Archer");
        prefabData[EntityType.archer_2] = Resources.Load<GameObject>("Battle/Prefabs/Archer");
        prefabData[EntityType.archer_3] = Resources.Load<GameObject>("Battle/Prefabs/Archer");

        prefabData[EntityType.wizard_1] = Resources.Load<GameObject>("Battle/Prefabs/Wizard");
        prefabData[EntityType.wizard_2] = Resources.Load<GameObject>("Battle/Prefabs/Wizard");
        prefabData[EntityType.wizard_3] = Resources.Load<GameObject>("Battle/Prefabs/Wizard");

        prefabData[EntityType.soldier_1] = Resources.Load<GameObject>("Battle/Prefabs/Soldier");
        prefabData[EntityType.soldier_2] = Resources.Load<GameObject>("Battle/Prefabs/Soldier");
        prefabData[EntityType.soldier_3] = Resources.Load<GameObject>("Battle/Prefabs/Soldier");
    }

    private void ApplyMapData()
    {
        mapDatas[MapType.firstStage_one] = Resources.Load<GameObject>("Battle/Prefabs/EasyMap01");
    }


}
