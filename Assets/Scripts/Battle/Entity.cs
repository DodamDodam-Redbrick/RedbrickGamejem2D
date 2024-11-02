using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntityStats
{
    public EntityStats DeepCopy()
    {
        EntityStats other = (EntityStats)this.MemberwiseClone();

        return other;
    }

    public EntityStats(float hp, float damage, float def, float moveSpeed, float fireRate, float skillCoolTime, int weight)
    {
        this.hp = hp;
        this.damage = damage;
        this.def = def;
        this.moveSpeed = moveSpeed;
        this.fireRate = fireRate;
        this.skillCoolTime = skillCoolTime;
        this.weight = weight;
    }

    public float hp;
    public float damage;
    public float def;
    public float moveSpeed;
    public float fireRate;
    public float skillCoolTime;
    public int weight;
}

[System.Serializable]
public class Entity
{
    public GameObject entityPrefab;

    public GameObject bulletPrefab;

    public EntityStats entityStats;
}