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

public static class Data
{
    public static Dictionary<EntityIndex, Entity> EntityData;
    public static Dictionary<RewardIndex, Reward> RewardData;
}
