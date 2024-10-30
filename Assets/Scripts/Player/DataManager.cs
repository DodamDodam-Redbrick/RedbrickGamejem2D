using System;
using System.Collections;
using System.Collections.Generic;
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

public class Reward
{
    public string imagePath;
    public string description;
    public RewardType type;
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

    private void Awake()
    {
        DontDestroyOnLoad(this);

        Instance = this;

        ImageData[ImageIndex.map_boss] = bossSprite;
        ImageData[ImageIndex.map_battle] = battleSprite;
        ImageData[ImageIndex.map_randomEvent] = randomEventSprite;
        ImageData[ImageIndex.map_shop] = shopSprite;
        ImageData[ImageIndex.map_start] = startSprite;
    }
}