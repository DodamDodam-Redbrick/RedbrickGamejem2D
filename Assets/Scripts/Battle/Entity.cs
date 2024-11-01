using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EntityStats
{
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

public class Entity
{
    public MapGrid mapGrid;

    public GameObject entityPrefab;

    public GameObject bulletPrefab;

    public EntityStats entityStats;
}