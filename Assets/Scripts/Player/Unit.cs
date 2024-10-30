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

public class Unit : Entity
{
    UnitType unitType;

    Sprite portrait;

    Node placedNode;

    double skillCoolTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}