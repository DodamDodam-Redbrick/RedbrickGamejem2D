using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EntityStats
{
    public EntityStats(double hp, double damage, double def, double moveSpeed, double fireRate, double skillCoolTime, int weight)
    {
        this.hp = hp;
        this.damage = damage;
        this.def = def;
        this.moveSpeed = moveSpeed;
        this.fireRate = fireRate;
        this.skillCoolTime = skillCoolTime;
        this.weight = weight;
    }

    double hp;
    double damage;
    double def;
    double moveSpeed;
    double fireRate;
    double skillCoolTime;
    int weight;
}

public class Entity
{
    public MapGrid mapGrid;

    public GameObject entityPrefab;

    public GameObject bulletPrefab;

    public EntityStats entityStats;
}