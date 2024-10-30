using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Entity : MonoBehaviour
{
    public Entity()
    {
    }

    public void SetStat(string path, double hp, double damage, double def, double moveSpeed, double fireRate, int weight)
    {
        this.prefabPath = path;
        this.hp = hp;
        this.damage = damage;
        this.def = def;
        this.moveSpeed = moveSpeed;
        this.fireRate = fireRate;
        this.weight = weight;
    }

    public GameObject unitPrefab;

    public GameObject bulletPrefab;

    public MapGrid mapGrid;

    public string prefabPath;
    //프리팹 소환할 때 기본 수치들 적용시켜주기
    public double hp;
    public double damage;
    public double def;
    public double moveSpeed;
    public double fireRate;
    public int weight; //저지 가능 수 (적은 보통 1)

    private void Start()
    {
        mapGrid = transform.parent.GetComponent<MapGrid>();
    }
}