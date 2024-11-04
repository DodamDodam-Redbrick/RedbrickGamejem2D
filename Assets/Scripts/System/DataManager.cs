using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum EntityType
{
    //아군 엔티티
    mainCharacter = 0,

    sword = 1,
    sword_1 = 10,
    sword_2 = 11,
    sword_3 = 12,

    archer = 2,
    archer_1 = 20,
    archer_2 = 21,
    archer_3 = 22,

    wizard = 3, //마스킹용
    wizard_1 = 30,
    wizard_2 = 31,
    wizard_3 = 32,

    soldier = 4,
    soldier_1 = 40,
    soldier_2 = 41,
    soldier_3 = 42,

    //적 엔티티
    wolf_1 = 100,
    wolf_2 = 101,
    wolf_3 = 102,

    bat_1 = 110,
    bat_2 = 111,
    bat_3 = 112,

    branchMonkey_1 = 120,
    branchMonkey_2 = 121,
    branchMonkey_3 = 122,

    rockMonkey_1 = 130,
    rockMonkey_2 = 131,
    rockMonkey_3 = 132,

    bear = 140,

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
    wolf_1 = EntityType.wolf_1,
    wolf_2 = EntityType.wolf_2,
    wolf_3 = EntityType.wolf_3,

    bat_1 = EntityType.bat_1,
    bat_2 = EntityType.bat_2,
    bat_3 = EntityType.bat_3,

    branchMonkey_1 = EntityType.branchMonkey_1,
    branchMonkey_2 = EntityType.branchMonkey_2,
    branchMonkey_3 = EntityType.branchMonkey_3,

    rockMonkey_1 = EntityType.rockMonkey_1,
    rockMonkey_2 = EntityType.rockMonkey_2,
    rockMonkey_3 = EntityType.rockMonkey_3,

    bear = EntityType.bear,
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

    unit_enemyWolf_1 = EntityType.wolf_1,
    unit_enemyWolf_2 = EntityType.wolf_2,
    unit_enemyWolf_3 = EntityType.wolf_3,

    unit_enemyBat_1 = EntityType.bat_1,
    unit_enemyBat_2 = EntityType.bat_2,
    unit_enemyBat_3 = EntityType.bat_3,

    unit_enemyBranchMonkey_1 = EntityType.branchMonkey_1,
    unit_enemyBranchMonkey_2 = EntityType.branchMonkey_2,
    unit_enemyBranchMonkey_3 = EntityType.branchMonkey_3,

    unit_enemyRockMonkey_1 = EntityType.rockMonkey_1,
    unit_enemyRockMonkey_2 = EntityType.rockMonkey_2,
    unit_enemyRockMonkey_3 = EntityType.rockMonkey_3,

    unit_enemyBear = EntityType.bear,

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

    firstStage_Boss,
}

public enum EventType
{
    one,
    two,

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

    [SerializedDictionary("RewardChances", "RewardType")]
    public SerializedDictionary<RewardType, float> rewardChanceDatas = new SerializedDictionary<RewardType, float>();

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

        imageData[ImageIndex.unit_soldier_1] = Resources.Load<Sprite>("Battle/Sprites/Soldier");
        imageData[ImageIndex.unit_soldier_2] = Resources.Load<Sprite>("Battle/Sprites/Soldier");
        imageData[ImageIndex.unit_soldier_3] = Resources.Load<Sprite>("Battle/Sprites/Soldier");

        // Enemy Image
        imageData[ImageIndex.unit_enemyWolf_1] = Resources.Load<Sprite>("Battle/Sprites/Wolf");
        imageData[ImageIndex.unit_enemyWolf_2] = Resources.Load<Sprite>("Battle/Sprites/Wolf");
        imageData[ImageIndex.unit_enemyWolf_3] = Resources.Load<Sprite>("Battle/Sprites/Wolf");

        imageData[ImageIndex.unit_enemyBat_1] = Resources.Load<Sprite>("Battle/Sprites/Bat");
        imageData[ImageIndex.unit_enemyBat_2] = Resources.Load<Sprite>("Battle/Sprites/Bat");
        imageData[ImageIndex.unit_enemyBat_3] = Resources.Load<Sprite>("Battle/Sprites/Bat");

        imageData[ImageIndex.unit_enemyBranchMonkey_1] = Resources.Load<Sprite>("Battle/Sprites/BranchMonkey");
        imageData[ImageIndex.unit_enemyBranchMonkey_2] = Resources.Load<Sprite>("Battle/Sprites/BranchMonkey");
        imageData[ImageIndex.unit_enemyBranchMonkey_3] = Resources.Load<Sprite>("Battle/Sprites/BranchMonkey");

        imageData[ImageIndex.unit_enemyRockMonkey_1] = Resources.Load<Sprite>("Battle/Sprites/RockMonkey");
        imageData[ImageIndex.unit_enemyRockMonkey_2] = Resources.Load<Sprite>("Battle/Sprites/RockMonkey");
        imageData[ImageIndex.unit_enemyRockMonkey_3] = Resources.Load<Sprite>("Battle/Sprites/RockMonkey");

        imageData[ImageIndex.unit_enemyBear] = Resources.Load<Sprite>("Battle/Sprites/Bear");

        // Shop
        imageData[ImageIndex.shop_potion_one] = Resources.Load<Sprite>("Shop/Sprites/Potion_one");
        imageData[ImageIndex.shop_potion_two] = Resources.Load<Sprite>("Shop/Sprites/Potion_Two");
        imageData[ImageIndex.shop_potion_three] = Resources.Load<Sprite>("Shop/Sprites/Potion_three");
    }

    private void ApplyPrefabDatas()
    {

        prefabData[EntityType.mainCharacter] = Resources.Load<GameObject>("Battle/Prefabs/MainCha");

        prefabData[EntityType.sword_1] = Resources.Load<GameObject>("Battle/Prefabs/Unit/Sword");
        prefabData[EntityType.sword_2] = Resources.Load<GameObject>("Battle/Prefabs/Unit/Sword");
        prefabData[EntityType.sword_3] = Resources.Load<GameObject>("Battle/Prefabs/Unit/Sword");

        prefabData[EntityType.archer_1] = Resources.Load<GameObject>("Battle/Prefabs/Unit/Archer");
        prefabData[EntityType.archer_2] = Resources.Load<GameObject>("Battle/Prefabs/Unit/Archer");
        prefabData[EntityType.archer_3] = Resources.Load<GameObject>("Battle/Prefabs/Unit/Archer");

        prefabData[EntityType.wizard_1] = Resources.Load<GameObject>("Battle/Prefabs/Unit/Wizard");
        prefabData[EntityType.wizard_2] = Resources.Load<GameObject>("Battle/Prefabs/Unit/Wizard");
        prefabData[EntityType.wizard_3] = Resources.Load<GameObject>("Battle/Prefabs/Unit/Wizard");

        prefabData[EntityType.soldier_1] = Resources.Load<GameObject>("Battle/Prefabs/Unit/Soldier");
        prefabData[EntityType.soldier_2] = Resources.Load<GameObject>("Battle/Prefabs/Unit/Soldier");
        prefabData[EntityType.soldier_3] = Resources.Load<GameObject>("Battle/Prefabs/Unit/Soldier");
    }

    private void ApplyMapData()
    {
        mapDatas[MapType.firstStage_one] = Resources.Load<GameObject>("Battle/Prefabs/Map/EasyMap01");
        mapDatas[MapType.firstStage_two] = Resources.Load<GameObject>("Battle/Prefabs/Map/Map2");

        mapDatas[MapType.firstStage_Boss] = Resources.Load<GameObject>("Battle/Prefabs/Map/Map3");
    }


}
