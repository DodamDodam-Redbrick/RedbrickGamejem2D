using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityIndex
{
    //�Ʊ� ��ƼƼ
    tank = 0,
    sword = 1,
    bow = 2,
    caster = 3,
    healer = 4,

    //�� ��ƼƼ
    slime = 101,
    wolf = 102,
}

public enum RewardIndex
{
    gold = 0,

}

public enum ImageIndex
{
    map_boss,
    map_battle,
    map_shop,
    map_randomEvent,
    map_start,
}

public class Entity
{
    public Entity(string path, double hp, double damage, double def, double moveSpeed, int weight)
    {
        this.prefabPath = path;
        this.hp = hp;
        this.damage = damage;
        this.def = def;
        this.moveSpeed = moveSpeed;
        this.weight = weight;
    }

    public string prefabPath;
    //������ ��ȯ�� �� �⺻ ��ġ�� ��������ֱ�
    public double hp;
    public double damage;
    public double def;
    public double moveSpeed;
    public int weight; //���� ���� �� (���� ���� 1)
}

public class Reward
{
    public string imagePath;
    public string description;
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

    public static Dictionary<EntityIndex, Entity> EntityData = new Dictionary<EntityIndex, Entity>();
    public static Dictionary<RewardIndex, Reward> RewardData = new Dictionary<RewardIndex, Reward>();
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