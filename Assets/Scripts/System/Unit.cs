using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitInfo : Entity
{
    public UnitInfo(EntityStats entityStats, UnitType unitType, Sprite thumbnail, GameObject unitPrefab, int cost, NodeType nodeType, GameObject bulletPrefab = null)
    {
        base.entityStats = entityStats;
        this.unitType = unitType;
        this.thumbnail = thumbnail;
        this.cost = cost;
        this.placeNodeType = nodeType;

        base.entityPrefab = unitPrefab;
        base.bulletPrefab = bulletPrefab;
    }

    public int cost;

    public UnitType unitType;

    [HideInInspector]
    public NodeType placeNodeType;

    [HideInInspector]
    public Sprite thumbnail;

}

public class Unit : MonoBehaviour
{
    public Unit(UnitInfo unitInfo)
    {
        this.unitInfo = unitInfo;
    }

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

