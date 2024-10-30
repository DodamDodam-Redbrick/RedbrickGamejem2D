using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitType
{
    tanker,
    sword,
    ranger,
    healer,
}

public class UnitInfo : Entity
{
    public UnitInfo(EntityStats entityStats, UnitType unitType, Sprite thumbnail, GameObject unitPrefab, GameObject bulletPrefab = null)
    {
        base.entityStats = entityStats;
        this.unitType = unitType;
        this.thumbnail = thumbnail;

        base.entityPrefab = unitPrefab;
        base.bulletPrefab = bulletPrefab;
    }

    public UnitType unitType;

    public Sprite thumbnail;

}

public class Unit : MonoBehaviour
{
    public UnitInfo unitInfo;

    public Node placedNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
