using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    //�Ʊ� ��ƼƼ
    tank = 0,
    sword = 1,
    bow = 2,
    caster = 3,
    healer = 4,

    //�� ��ƼƼ
    slime = 100,
    wolf = 101,

    //�Ѿ� ��ƼƼ
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

    public static Dictionary<RewardType, Reward> rewardData = new Dictionary<RewardType, Reward>(); //���� �ʿ��� ������
    public static Dictionary<ImageIndex, Sprite> imageData = new Dictionary<ImageIndex, Sprite>(); //�� �̹����� �����ϴ� ��
    public static Dictionary<EntityType, GameObject> prefabData = new Dictionary<EntityType, GameObject>(); //�� �����յ� �����ϴ� ��
    public static Dictionary<EntityType, Entity> entityData = new Dictionary<EntityType, Entity>(); //��ȯ�� �� ������ �� �ʱ�ȭ ��

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

        //�߿�!! �̹��� �����Ͷ� ������ �����ͺ��� �ڿ��ð�
        EntityStats swordStat = new EntityStats(100, 5, 1, 1, 1, 10, 1);
        entityData[EntityType.sword] = new UnitInfo(swordStat, UnitType.sword, imageData[ImageIndex.unit_sword_thumbnail], swordPrefab);
    }
}